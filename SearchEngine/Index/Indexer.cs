using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SearchEngine.Analysis;
using SearchEngine.Configuration;

namespace SearchEngine.Index
{
    /// <summary>
    /// 文档索引管理器
    /// </summary>
    public class Indexer
    {
        private Writer writer;

        /// <summary>
        /// 像索引中写入
        /// </summary>
        /// <param name="doc">待写入的文档</param>
        public void Write(Lucene.Net.Documents.Document doc)
        {
            writer.Write(doc);
        }

        /// <summary>
        /// 删除索引中符合条件的文档
        /// </summary>
        /// <param name="term">删除条件</param>
        public void Delete(Lucene.Net.Index.Term term)
        {
            writer.Delete(term);
        }

        /// <summary>
        /// 更新（添加）索引中的文档
        /// </summary>
        /// <param name="doc">更新文档，索引器会自动找到相同ID号的文档进行更新</param>
        public void Update(Lucene.Net.Documents.Document doc)
        {
            writer.Update(doc);
        }

        /// <summary>
        /// 文档索引管理器构造函数
        /// </summary>
        public Indexer()
        {
            writer = new Writer();
        }

        private sealed class Writer
        {
            /// <summary>
            /// 索引编写器
            /// </summary>
            public static Lucene.Net.Index.IndexWriter writer;
            /// <summary>
            /// 最大缓冲文档数
            /// </summary>
            public int maxBufferLength = 0x100;
            /// <summary>
            /// 执行线程
            /// </summary>
            public Thread thread;
            /// <summary>
            /// 添加任务队列
            /// </summary>
            public Queue<Lucene.Net.Documents.Document> addQueue;
            /// <summary>
            /// 更新任务队列
            /// </summary>
            public Queue<Lucene.Net.Documents.Document> updateQueue;
            /// <summary>
            /// 删除任务队列
            /// </summary>
            public Queue<Lucene.Net.Index.Term> deleteQueue;

            /// <summary>
            /// 文档写入器构造函数
            /// </summary>
            public Writer()
            {
                addQueue = new Queue<Lucene.Net.Documents.Document>();
                updateQueue = new Queue<Lucene.Net.Documents.Document>();
                deleteQueue = new Queue<Lucene.Net.Index.Term>();

                thread = new Thread(new ThreadStart(IndexWriteHandler));
                thread.IsBackground = true;
                thread.Priority = ThreadPriority.BelowNormal;
                thread.Start();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="bufferLength"></param>
            public Writer(int bufferLength)
                : this()
            {
                SetBufferLength(bufferLength);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="bufferLength"></param>
            public void SetBufferLength(int bufferLength)
            {
                this.maxBufferLength = bufferLength;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="doc"></param>
            public void Write(Lucene.Net.Documents.Document doc)
            {
                this.addQueue.Enqueue(doc);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="term"></param>
            public void Delete(Lucene.Net.Index.Term term)
            {
                this.deleteQueue.Enqueue(term);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="doc"></param>
            public void Update(Lucene.Net.Documents.Document doc)
            {
                this.updateQueue.Enqueue(doc);
            }
            ~Writer()
            {
                try
                {
                    writer.Optimize();
                    writer.Close();
                }
                catch
                {
                    File.Delete(Directorys.IndexDirectory + "write.lock");
                }
            }

            /// <summary>
            /// 任务执行器
            /// </summary>
            public void IndexWriteHandler()
            {
                //创建文档索引编写器
                writer = new Lucene.Net.Index.IndexWriter(Directorys.IndexDirectory, new ThesaurusAnalyzer(), !File.Exists(Directorys.IndexDirectory + "segments.gen"));
                //设置最大碎片缓冲
                writer.SetMaxBufferedDocs(maxBufferLength);
                //首次启动优化
                writer.Optimize();
                int count = 0;

                //处理循环
                while (true)
                {
                    //处理删除队列
                    while (deleteQueue.Count > 0 && count < maxBufferLength)
                    {
                        count++;
                        writer.DeleteDocuments(deleteQueue.Dequeue());
                    }
                    //处理更新队列
                    while (updateQueue.Count > 0 && count < maxBufferLength)
                    {
                        count++;
                        Lucene.Net.Documents.Document doc = updateQueue.Dequeue();
                        writer.UpdateDocument(new Lucene.Net.Index.Term("id", doc.Get("id")), doc);
                    }
                    //处理新增队列
                    while (addQueue.Count > 0 && count < maxBufferLength)
                    {
                        count++;
                        writer.AddDocument(addQueue.Dequeue());
                    }
                    //如果有入档则保存碎片
                    if (writer.NumRamDocs() > 0)
                    {
                        writer.Flush();
                    }
                    //检测处理次数是否达到最大缓冲数,当超过最大缓冲数时优化碎片,否则线程暂停100毫秒
                    if (count >= maxBufferLength)
                    {
                        writer.Optimize();
                        count = 0;
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }
}
