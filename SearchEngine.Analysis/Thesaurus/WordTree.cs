using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SearchEngine.Analysis.Thesaurus
{
    public class WordTree
    {
        private TreeNode root = null;

        private Hashtable map = new Hashtable();

        private WordTree()
        {
            root = new TreeNode();
        }

        internal static WordTree CreateWordTree()
        {
            return new WordTree();
        }

        public TreeNode Root
        {
            get
            {
                return this.root;
            }
        }

        public bool ContainsKey(char character)
        {
            return this.map.ContainsKey(character);
        }

        public void Add(String word)
        {
            TreeNode tempNode = this.root;
            char[] charArray = word.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                TreeNode node = new TreeNode();
                node.Character = charArray[i];
                if (!tempNode.ContainsChild(node))
                {
                    tempNode.AppendChild(node);
                    tempNode = node;
                }
                else
                {
                    tempNode = tempNode.GetChildren(charArray[i]);
                }
            }
            tempNode.IsWordEnd = true;
        }

        public bool Contains(String word)
        {
            TreeNode tempNode = this.root;
            char[] charArray = word.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                TreeNode node = new TreeNode();
                node.Character = charArray[i];
                if (!tempNode.ContainsChild(node))
                    return false;
                else
                    tempNode = tempNode.GetChildren(charArray[i]);
            }
            if (tempNode.IsWordEnd)
                return true;
            else
                return false;
        }
    }
}
