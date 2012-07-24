using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SearchEngine.Analysis.Thesaurus
{
    public class TreeNode
    {
        private char character;

        private bool isWordEnd = false;

        private TreeNode parent = null;

        private Hashtable children = new Hashtable();

        public char Character
        {
            set
            {
                this.character = value;
            }
            get
            {
                return this.character;
            }
        }
        public bool IsWordEnd
        {
            set
            {
                this.isWordEnd = value;
            }
            get
            {
                return isWordEnd;
            }
        }

        public TreeNode ParentNode
        {
            set
            {
                this.parent = value;
            }
            get
            {
                return this.parent;
            }
        }

        public bool HasChildren
        {
            get
            {
                return this.children.Count > 0;
            }
        }

        public int Count
        {
            get
            {
                return this.children.Count;
            }
        }


        public TreeNode GetChildren(char key)
        {
            if(children.ContainsKey(key))
            {
                return (TreeNode)children[key];
            }
            return null;
        }

        public bool ContainsKey(char key)
        {
            return children.ContainsKey(key);
        }

        public TreeNode()
        {
        }

        public TreeNode(char character, bool isWordEnd, TreeNode parent)
        {
            this.character = character;
            this.isWordEnd = isWordEnd;
            this.parent = parent;
        }

        public void AppendChild(TreeNode child)
        {
            child.parent = this;
            this.children.Add(child.character, child);
        }

        public bool ContainsChild(TreeNode child)
        {
            return this.children.ContainsKey(child.character);
        }

        public void RemoveChild(TreeNode child)
        {
            this.children.Remove(child.character);
        }

        public void Clear()
        {
            this.children.Clear();
        }

        public override String ToString()
        {
            string word = String.Empty;
            TreeNode tempNode = this;
            if (this.isWordEnd)
            {
                while (tempNode.parent != null)
                {

                    word = tempNode.character.ToString() + word;

                    tempNode = tempNode.parent;
                }
            }

            return word;
        }
    }
}
