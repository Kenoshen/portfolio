using System;
using System.Collections.Generic;
using System.Text;

namespace Winger.Utils
{
    /// <summary>
    /// A generic tree structure.  Good for data structures similar to file systems.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Tree<T>
    {
        private Tree<T> parent = null;
        private T self;
        private List<Tree<T>> children = new List<Tree<T>>();

        private string verticalLine = "|";
        private string horizontalLine = "_";
        private string diagonalLine = "\\";
        private int width = 5;

        private int infiniteLoopSafety = 100;


        /// <summary>
        /// A basic tree structure.  Each tree object has a parent(Tree<T>) and a list of children(List<Tree<T>>) and a self(T).
        /// </summary>
        /// <param name="self"></param>
        public Tree(T self)
        {
            SetSelf(self);
        }


        #region Checker Methods

        /// <summary>
        /// Checks if the tree object is a trunk. (no parent)
        /// </summary>
        /// <returns></returns>
        public bool IsTrunk()
        {
            return (parent == null);
        }


        /// <summary>
        /// Checks if the tree object is a branch. (has parent and children)
        /// </summary>
        /// <returns></returns>
        public bool IsBranch()
        {
            return (parent != null && children.Count > 0);
        }


        /// <summary>
        /// Checks if the tree object is a leaf. (has parent and no children)
        /// </summary>
        /// <returns></returns>
        public bool IsLeaf()
        {
            return (parent != null && children.Count == 0);
        }


        /// <summary>
        /// Checks if two tree objects have equal selfs.
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public bool IsSelfEqual(Tree<T> tree)
        {
            if (tree != null)
            {
                if (self != null)
                {
                    return (self.Equals(tree.self));
                }
                else
                {
                    return (tree.self == null);
                }
            }
            return false;
        }

        #endregion


        #region Setter Methods
        /// <summary>
        /// Sets the self of this tree
        /// </summary>
        /// <param name="self"></param>
        public void SetSelf(T self)
        {
            this.self = self;
        }
        #endregion


        #region Getter Methods

        /// <summary>
        /// Gets the number of children tree objets from this tree object.
        /// </summary>
        /// <returns></returns>
        public int GetChildCount()
        {
            return children.Count;
        }


        /// <summary>
        /// Gets the total number of leaf tree objects from this tree 
        /// object and from this tree object's children recursively.
        /// </summary>
        /// <returns></returns>
        public int GetTreeLeafCount()
        {
            if (IsLeaf())
            {
                return 1;
            }
            else
            {
                return this.RecurseToFindLeafCount(this, 0, GetDepth());
            }
        }


        /// <summary>
        /// Gets the total number of branch tree objects from this tree 
        /// object and from this tree object's children recursively.
        /// </summary>
        /// <returns></returns>
        public int GetTreeBranchCount()
        {
            if (IsLeaf())
            {
                return 0;
            }
            else
            {
                return this.RecurseToFindBranchCount(this, 0, GetDepth());
            }
        }


        /// <summary>
        /// Gets the parent of this tree object.
        /// </summary>
        /// <returns></returns>
        public Tree<T> GetParent()
        {
            return parent;
        }


        /// <summary>
        /// Gets the self of this tree object.
        /// </summary>
        /// <returns></returns>
        public T GetSelf()
        {
            return self;
        }


        /// <summary>
        /// Gets a child of this tree object at a given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Tree<T> GetChild(int index)
        {
            return children[index];
        }


        /// <summary>
        /// Tries to find the given self that matches with the self of a 
        /// child and returns the matching child, or null if there is no match.
        /// </summary>
        /// <param name="otherSelf"></param>
        /// <returns></returns>
        public Tree<T> GetChildBySelf(T otherSelf)
        {
            for (int i = 0; i < GetChildCount(); i++)
            {
                if (GetChild(i).GetSelf().Equals(otherSelf))
                {
                    return GetChild(i);
                }
            }

            return null;
        }


        /// <summary>
        /// Gets the list of children tree objects from this tree object.
        /// </summary>
        /// <returns></returns>
        public List<Tree<T>> GetChildren()
        {
            return children;
        }


        /// <summary>
        /// Gets a list of all the selfs of each child tree object.
        /// </summary>
        /// <returns></returns>
        public List<T> GetChildrenSelfs()
        {
            List<T> selfs = new List<T>();
            for (int i = 0; i < GetChildCount(); i++)
            {
                selfs.Add(GetChild(i).GetSelf());
            }
            return selfs;
        }


        /// <summary>
        /// Gets the current depth of this tree object. Or in other words, 
        /// it gets the number of steps to get from this tree object to 
        /// the trunk tree object.
        /// </summary>
        /// <returns></returns>
        public int GetDepth()
        {
            if (IsTrunk())
            {
                return 0;
            }
            else
            {
                return RecurseToFindDepth(GetParent(), 0);
            }
        }


        /// <summary>
        /// Gets a list of objects in order from the trunk of the tree to 
        /// this tree objects parent.  Returns an empty list if this tree 
        /// object is the trunk.
        /// </summary>
        /// <returns></returns>
        public List<Tree<T>> GetAncestry()
        {
            if (IsTrunk())
            {
                return new List<Tree<T>>();
            }
            else
            {
                return RecurseToFindAncestry(GetParent(), new List<Tree<T>>(), 0);
            }
        }


        /// <summary>
        /// Calls the toString() method on each object in the list from the 
        /// getAncestry() method as well as this tree object itself and 
        /// concatenates a string using a given string separator.
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string GetPath(String separator)
        {
            StringBuilder sb = new StringBuilder();
            List<Tree<T>> ancestry = GetAncestry();
            for (int i = 0; i < ancestry.Count; i++)
            {
                sb.Append(ancestry[i].GetSelf().ToString());
                sb.Append(separator);
            }
            sb.Append(GetSelf().ToString());
            return sb.ToString();
        }


        /// <summary>
        /// Get the leaf objects from this tree object down.
        /// </summary>
        /// <returns></returns>
        public List<Tree<T>> GetLeaves()
        {
            List<Tree<T>> leaves = new List<Tree<T>>();
            if (IsLeaf())
            {
                leaves.Add(this);
            }
            else
            {
                RecurseToFindLeaves(this, leaves, 0);
            }
            return leaves;
        }


        /// <summary>
        /// Gets a list of all the tree objects in a breadth first order
        /// </summary>
        /// <returns></returns>
        public List<T> GetBreadthFirstList()
        {
            return RecurseToBreadthFirst(this);
        }


        /// <summary>
        /// Gets a list of all the tree objects in a depth first order
        /// </summary>
        /// <returns></returns>
        public List<T> GetDepthFirstList()
        {
            List<T> objs = new List<T>();
            return RecurseToDepthFirst(this, objs, 0);
        }

        /// <summary>
        /// Gets a list of the tree objects in the tree
        /// </summary>
        /// <returns></returns>
        public List<Tree<T>> GetTreeList()
        {
            List<Tree<T>> objs = new List<Tree<T>>();
            return RecurseForTreeList(this, objs, 0);
        }

        #endregion


        #region Adder Methods

        /// <summary>
        /// Adds a list of objects where each consecutive tree object is a child of 
        /// the previous tree object and returns the last object as a tree.
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        public Tree<T> AddBranch(params T[] branch)
        {
            Tree<T> currentTree = this;
            for (int i = 0; i < branch.Length; i++)
            {
                Tree<T> nextTree = currentTree.AddLeaf(branch[i]); ;
                currentTree = nextTree;
            }
            return currentTree;
        }


        /// <summary>
        /// Adds a list of objects where each consecutive tree object is a child of 
        /// the previous tree object and returns the last object as a tree.
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        public Tree<T> AddBranch(params Tree<T>[] branch)
        {
            Tree<T> currentTree = this;
            for (int i = 0; i < branch.Length; i++)
            {
                Tree<T> nextTree = currentTree.AddLeaf(branch[i]); ;
                currentTree = nextTree;
            }
            return currentTree;
        }


        /// <summary>
        /// Adds a list of objects where each consecutive tree object is a child of 
        /// the previous tree object and returns the last object as a tree.
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        public Tree<T> AddBranch(List<T> branch)
        {
            Tree<T> currentTree = this;
            for (int i = 0; i < branch.Count; i++)
            {
                Tree<T> nextTree = currentTree.AddLeaf(branch[i]); ;
                currentTree = nextTree;
            }
            return currentTree;
        }


        /// <summary>
        /// Adds a single object to the list of children.
        /// </summary>
        /// <param name="leaf"></param>
        /// <returns></returns>
        public Tree<T> AddLeaf(T leaf)
        {
            return AddLeaf(new Tree<T>(leaf));
        }


        /// <summary>
        /// Adds a single object to the list of children.
        /// </summary>
        /// <param name="leaf"></param>
        /// <returns></returns>
        public Tree<T> AddLeaf(Tree<T> leaf)
        {
            for (int i = 0; i < GetChildCount(); i++)
            {
                if (GetChild(i).IsSelfEqual(leaf))
                {
                    return GetChild(i);
                }
            }
            leaf.SetParent(this);
            children.Add(leaf);
            return leaf;
        }

        #endregion


        #region Remover Methods

        /// <summary>
        /// Removes the tree from its parent.getChildren() list.
        /// </summary>
        public void RemoveFromTreeStructure()
        {
            if (!IsTrunk())
            {
                List<Tree<T>> siblings = parent.GetChildren();
                int selfIndex = -1;
                for (int i = 0; i < siblings.Count; i++)
                {
                    if (siblings[i].IsSelfEqual(this))
                    {
                        selfIndex = i;
                        break;
                    }
                }
                if (selfIndex >= 0)
                {
                    parent.RemoveChild(selfIndex);
                    SetParent(null);
                }
            }
        }


        /// <summary>
        /// Removes a child from this tree objects list of children.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Tree<T> RemoveChild(int index)
        {
            Tree<T> child = GetChild(index);
            children.RemoveAt(index);
            return child;
        }

        #endregion


        #region Customize Logging Methods

        /// <summary>
        /// Customize the look of the vertical line for the ToString() method.
        /// </summary>
        /// <param name="s"></param>
        public void CustomizeVerticalLine(string s)
        {
            verticalLine = s;
        }


        /// <summary>
        /// Customize the look of the horizontal line for the ToString() method.
        /// </summary>
        /// <param name="s"></param>
        public void CustomizeHorizontalLine(string s)
        {
            horizontalLine = s;
        }


        /// <summary>
        /// Customize the look of the diagonal line for the ToString() method.
        /// </summary>
        /// <param name="s"></param>
        public void CustomizeDiagonalLine(string s)
        {
            diagonalLine = s;
        }


        /// <summary>
        /// Customize the number of characters in the columns of each line of the ToString() method.
        /// </summary>
        /// <param name="w"></param>
        public void CustomizeWidth(int w)
        {
            if (w < 2)
            {
                width = 2;
            }
            else
            {
                width = w;
            }
        }

        #endregion


        #region Override Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result
                    + ((children == null) ? 0 : children.GetHashCode());
            result = prime * result + ((parent == null) ? 0 : parent.GetHashCode());
            result = prime * result + ((self == null) ? 0 : self.GetHashCode());
            return result;
        }


        /// <summary>
        /// Checks if the entire tree structure and it's immediate children 
        /// are equal to each other.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Tree<T>))
            {
                return false;
            }
            Tree<T> other = (Tree<T>)obj;
            if (children == null)
            {
                if (other.children != null)
                {
                    return false;
                }
            }
            else if (!children.Equals(other.children))
            {
                return false;
            }
            if (parent == null)
            {
                if (other.parent != null)
                {
                    return false;
                }
            }
            else if (!parent.Equals(other.parent))
            {
                return false;
            }
            if (self == null)
            {
                if (other.self != null)
                {
                    return false;
                }
            }
            else if (!self.Equals(other.self))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Outputs the objects in the tree to a formatted string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("\n");
            List<Boolean> lastChildList = new List<Boolean>();
            lastChildList.Add(false);
            RecurseToPrintString(this, sb, 0, lastChildList);
            return sb.ToString();
        }

        #endregion


        #region Private Methods

        private void SetParent(Tree<T> parent)
        {
            this.parent = parent;
        }


        private int RecurseToFindDepth(Tree<T> tree, int currentDepth)
        {
            if (tree == null || currentDepth >= infiniteLoopSafety)
            {
                return currentDepth;
            }
            else
            {
                currentDepth++;
                return RecurseToFindDepth(tree.GetParent(), currentDepth);
            }
        }


        private int RecurseToFindLeafCount(Tree<T> tree, int currentLeafCount, int currentDepth)
        {
            currentDepth++;
            if (currentDepth >= infiniteLoopSafety)
            {
                return currentLeafCount;
            }
            else if (tree.IsLeaf())
            {
                return currentLeafCount + 1;
            }
            else
            {
                for (int i = 0; i < tree.GetChildCount(); i++)
                {
                    currentLeafCount = RecurseToFindLeafCount(tree.GetChild(i), currentLeafCount, currentDepth);
                }
                return currentLeafCount;
            }
        }


        private void RecurseToFindLeaves(Tree<T> tree, List<Tree<T>> leaves, int currentDepth)
        {
            currentDepth++;
            if (currentDepth >= infiniteLoopSafety)
            {
                return;
            }
            else if (tree.IsLeaf())
            {
                leaves.Add(tree);
            }
            else
            {
                for (int i = 0; i < tree.GetChildCount(); i++)
                {
                    RecurseToFindLeaves(tree.GetChild(i), leaves, currentDepth);
                }
            }
        }


        private int RecurseToFindBranchCount(Tree<T> tree, int currentBranchCount, int currentDepth)
        {
            currentDepth++;
            if (tree.IsLeaf() || currentDepth >= infiniteLoopSafety)
            {
                return currentBranchCount;
            }
            else
            {
                currentBranchCount++;
                for (int i = 0; i < tree.GetChildCount(); i++)
                {
                    currentBranchCount = RecurseToFindLeafCount(tree.GetChild(i), currentBranchCount, currentDepth);
                }
                return currentBranchCount;
            }
        }


        private List<Tree<T>> RecurseToFindAncestry(Tree<T> tree, List<Tree<T>> ancestry, int currentDepth)
        {
            currentDepth++;
            if (currentDepth >= infiniteLoopSafety)
            {
                return ancestry;
            }
            else
            {
                ancestry.Insert(0, tree);
                if (!tree.IsTrunk())
                {
                    return RecurseToFindAncestry(tree.GetParent(), ancestry, currentDepth);
                }
                else
                {
                    return ancestry;
                }
            }
        }


        private void RecurseToPrintString(Tree<T> tree, StringBuilder sb, int currentDepth, List<bool> lastChildList)
        {
            if (currentDepth >= infiniteLoopSafety)
            {
                return;
            }

            List<bool> newLastChildList = new List<bool>();
            for (int i = 0; i < lastChildList.Count; i++)
            {
                newLastChildList.Add(lastChildList[i]);
            }


            for (int i = 0; i < currentDepth; i++)
            {
                if (i + 1 >= currentDepth)
                {
                    if (newLastChildList[currentDepth])
                    {
                        sb.Append(diagonalLine);
                        for (int k = 0; k < width - 2; k++)
                        {
                            sb.Append(horizontalLine);
                        }
                        sb.Append(horizontalLine);
                        //					sb.append("`--->");
                        //					sb.append("\\____");
                    }
                    else
                    {
                        sb.Append(verticalLine);
                        for (int k = 0; k < width - 2; k++)
                        {
                            sb.Append(horizontalLine);
                        }
                        sb.Append(horizontalLine);
                        //					sb.append("|--->");
                        //					sb.append("|____");
                    }
                }
                else
                {
                    if (newLastChildList[i + 1])
                    {
                        for (int k = 0; k < width; k++)
                        {
                            sb.Append(" ");
                        }
                        //					sb.append("     ");
                    }
                    else
                    {
                        sb.Append(verticalLine);
                        for (int k = 0; k < width - 1; k++)
                        {
                            sb.Append(" ");
                        }
                        //					sb.append("|    ");
                    }
                }
            }

            sb.Append(tree.GetSelf().ToString());
            sb.Append("\n");

            newLastChildList.Add(false);
            currentDepth++;

            for (int i = 0; i < tree.GetChildCount(); i++)
            {
                if (i + 1 >= tree.GetChildCount())
                {
                    newLastChildList[currentDepth] = true;
                }
                else
                {
                    newLastChildList[currentDepth] = false;
                }
                RecurseToPrintString(tree.GetChild(i), sb, currentDepth, newLastChildList);
            }
        }


        private List<T> RecurseToBreadthFirst(Tree<T> tree)
        {
            List<Tree<T>> queue = new List<Tree<T>>();
            List<T> objs = new List<T>();
            queue.Add(tree);

            while (queue.Count > 0)
            {
                Tree<T> curTree = queue[0];
                queue.RemoveAt(0);
                objs.Add(curTree.GetSelf());

                for (int i = 0; i < curTree.GetChildCount(); i++)
                {
                    Tree<T> childTree = curTree.GetChild(i);
                    bool found = false;
                    for (int k = 0; k < objs.Count; k++)
                    {
                        if (childTree.GetSelf().Equals(objs[k]))
                            found = true;
                    }
                    if (!found)
                        queue.Add(childTree);
                }
            }

            return objs;
        }


        private List<T> RecurseToDepthFirst(Tree<T> tree, List<T> objs, int currentDepth)
        {
            objs.Add(tree.GetSelf());

            currentDepth++;

            for (int i = 0; i < tree.GetChildCount(); i++)
            {
                List<T> temp = new List<T>();
                temp = RecurseToDepthFirst(tree.GetChild(i), objs, currentDepth);
                objs = temp;
            }

            return objs;
        }


        private List<Tree<T>> RecurseForTreeList(Tree<T> tree, List<Tree<T>> objs, int currentDepth)
        {
            objs.Add(tree);

            currentDepth++;

            for (int i = 0; i < tree.GetChildCount(); i++)
            {
                List<Tree<T>> temp = new List<Tree<T>>();
                temp = RecurseForTreeList(tree.GetChild(i), objs, currentDepth);
                objs = temp;
            }

            return objs;
        }

        #endregion
    }
}
