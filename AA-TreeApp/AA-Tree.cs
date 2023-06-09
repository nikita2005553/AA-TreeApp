﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA_TreeApp
{
    public class AANode<T> where T : IComparable<T>
    {
        public T Value { get; set; }
        public int Level { get; set; }
        public AANode<T> Left { get; set; }
        public AANode<T> Right { get; set; }

        public AANode(T value)
        {
            Value = value;
            Level = 1;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class AATree<T> where T : IComparable<T>
    {
        private AANode<T> _root;
        public int iteration = 0;

        public AATree()
        {
            _root = null;
        }

        public AATree(T value)
        {
            _root = new AANode<T>(value);
        }

        public AATree(T[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                this.Add(values[i]);
            }
        }

        public AATree(List<T> values)
        {
            T[] arr = values.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                this.Add(values[i]);
            }
        }


        public void Add(T value)
        {
            iteration = 0;
            _root = Add(value, _root);
        }


        private AANode<T> Add(T value, AANode<T> node)
        {

            if (node == null)
            {
                return new AANode<T>(value);
            }

            if (value.CompareTo(node.Value) < 0)
            {
                iteration++;
                node.Left = Add(value, node.Left);
            }
            else if (value.CompareTo(node.Value) > 0)
            {
                iteration++;
                node.Right = Add(value, node.Right);
            }
            else
            {
                iteration++;
                return node;
            }

            node = Skew(node);
            node = Split(node);

            return node;
        }

        private AANode<T> Skew(AANode<T> node)
        {
            if (node == null)
            {
                return node;
            }

            if (node.Left != null && node.Left.Level == node.Level)
            {
                AANode<T> left = node.Left;
                node.Left = left.Right;
                left.Right = node;
                node = left;
            }

            return node;
        }

        private AANode<T> Split(AANode<T> node)
        {
            if (node == null)
            {
                return node;
            }

            if (node.Right != null && node.Right.Right != null && node.Right.Level == node.Level)
            {
                AANode<T> right = node.Right;
                node.Right = right.Left;
                right.Left = node;
                right.Level++;
                node = right;
            }

            return node;
        }

        public void Remove(T value)
        {
            iteration = 0;
            _root = Remove(value, _root);
        }

        private AANode<T> Remove(T value, AANode<T> node)
        {
            if (node == null)
            {
                return node;
            }
            if (value.CompareTo(node.Value) < 0)
            {
                iteration++;
                node.Left = Remove(value, node.Left);
            }
            else if (value.CompareTo(node.Value) > 0)
            {
                iteration++;
                node.Right = Remove(value, node.Right);
            }
            else
            {
                if (node.Left == null)
                {
                    iteration++;
                    return node.Right;
                }
                else if (node.Right == null)
                {
                    iteration++;
                    return node.Left;
                }
                else
                {
                    AANode<T> minNode = FindMin(node.Right);
                    node.Value = minNode.Value;
                    node.Right = Remove(minNode.Value, node.Right);
                }
            }

            node = Skew(node);
            if (node.Right != null)
            {
                node.Right = Skew(node.Right);
            }
            if (node.Right != null && node.Right.Right != null)
            {
                node.Right.Right = Skew(node.Right.Right);
            }
            node = Split(node);
            if (node.Right != null)
            {
                node.Right = Split(node.Right);
            }

            return node;
        }

        private AANode<T> FindMin(AANode<T> node)
        {
            if (node == null)
            {
                return node;
            }

            while (node.Left != null)
            {
                node = node.Left;
            }

            return node;
        }

        public bool Contains(T value)
        {
            return Contains(value, _root);
        }

        private bool Contains(T value, AANode<T> node)
        {
            if (node == null)
            {
                return false;
            }

            if (value.CompareTo(node.Value) == 0)
            {
                return true;
            }
            else if (value.CompareTo(node.Value) < 0)
            {
                return Contains(value, node.Left);
            }
            else
            {
                return Contains(value, node.Right);
            }
        }

        public AANode<T> Search(T value, AANode<T> node)
        {
            if (node == null || value.CompareTo(node.Value) == 0)
            {
                iteration++;
                return node;
            }

            if (value.CompareTo(node.Value) < 0)
            {
                iteration++;
                return Search(value, node.Left);
            }
            else
            {
                iteration++;
                return Search(value, node.Right);
            }
        }

        public AANode<T> Search(T Value)
        {
            iteration = 0;
            return this.Search(Value, _root);
        }

        public void PrintTree()
        {
            PrintTree(_root);
        }

        public void PrintTree(AANode<T> node)
        {
            if (node == null) return;
            PrintTree(node.Left);
            Console.WriteLine(node.Value);
            PrintTree(node.Right);
        }
    }
}