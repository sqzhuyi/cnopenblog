using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SearchEngine.Analysis.Thesaurus
{
    public class AssociateStream
    {
        private WordTree tree;
        private TreeNode currNode;

        private int step = 0;
        private bool isOccurWord = false;

        public TreeNode Root
        {
            get
            {
                return tree.Root;
            }
        }
        public AssociateStream()
        {
            tree = WordTreeFactory.GetInstance();
            currNode = Root;
        }

        public bool IsBegin
        {
            get
            {
                return this.currNode.Equals(Root);
            }
        }

        public bool Associate(char nextChar)
        {
            if (currNode.ContainsKey(nextChar))
            {
                step++;
                this.currNode = currNode.GetChildren(nextChar);
                if (currNode.IsWordEnd)
                    this.isOccurWord = true;

                return true;
            }
            else
                return false;
        }

        public bool IsOccurWord
        {
            get
            {
                return this.isOccurWord;
            }
        }

        public bool HasChildren
        {
            get
            {
                return this.currNode.HasChildren;
            }
        }


        public bool IsWordEnd
        {
            get
            {
                return this.currNode.IsWordEnd;
            }
        }

        public bool BackToLastWordEnd()
        {
            TreeNode tempNode = currNode;
            do
            {
                tempNode = tempNode.ParentNode;
                step--;
                if (tempNode.IsWordEnd)
                {
                    currNode = tempNode;
                    return true;
                }
            } while (tempNode.ParentNode != null);
            return false;
        }

        public int Step
        {
            get
            {
                return this.step;
            }
        }

        public bool Move(string word)
        {
            char[] charArray = word.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                if (!Associate(charArray[i]))
                {
                    step -= i;
                    for(;i > 0;i--)
                        this.currNode = this.currNode.ParentNode;
                    return false;
                }
            }
            return this.currNode.IsWordEnd;
        }

        public void AddWord(string word)
        {
            this.tree.Add(word);
        }

        public void DeleteWord(string word)
        {
            Reset();
            if (Move(word) && currNode.IsWordEnd)
            {
                if (currNode.HasChildren)
                {
                    currNode.IsWordEnd = false;
                }
                else
                {
                    do
                    {
                        TreeNode tempNode = currNode.ParentNode;
                        tempNode.RemoveChild(currNode);
                        currNode = tempNode;
                    } while (!Root.Equals(currNode) && !currNode.IsWordEnd && !currNode.HasChildren);
                }
            }
            Reset();
        }

        public String GetWord()
        {
            return this.currNode.ToString();
        }

        public void Reset()
        {
            this.step = 0;
            this.currNode = this.Root;
            this.isOccurWord = false;
        }
    }
}
