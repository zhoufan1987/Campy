﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campy.Graphs
{
    public class TSortNoBackEdges
    {
        /// Topological Sorting (Kahn's algorithm) 
        public static IEnumerable<T> Sort<T, E>(IGraph<T, E> graph, T s)
            where E : IEdge<T>
        {
            var source = new T[] { s };
            Dictionary<T, bool> visited = new Dictionary<T, bool>();

            foreach (T v in graph.Vertices)
                visited.Add(v, false);

            if (source != null && source.Any())
            {
                EdgeClassifier.Classify(graph, s, out Dictionary<E, EdgeClassifier.Classification> result);
                HashSet<T> nodes = new HashSet<T>();
                foreach (T v in graph.Vertices) nodes.Add(v);
                HashSet<Tuple<T, T>> edges = new HashSet<Tuple<T, T>>();
                // Add only non-back edges.
                foreach (E e in graph.Edges)
                    if (result[e] != EdgeClassifier.Classification.Back)
                        edges.Add(new Tuple<T, T>(e.From, e.To));

                // Set of all nodes with no incoming edges
                var S = new HashSet<T>(nodes.Where(n => edges.All(e => e.Item2.Equals(n) == false)));

                // while S is non-empty do
                while (S.Any())
                {
                    //  remove a node n from S
                    var n = S.First();
                    S.Remove(n);

                    // add n to tail of L
                    yield return n;

                    // for each node m with an edge e from n to m do
                    var look = edges.Where(e => e.Item1.Equals(n)).ToList();
                    foreach (var e in look)
                    {
                        var m = e.Item2;

                        // remove edge e from the graph
                        edges.Remove(e);

                        // if m has no other incoming edges then
                        if (edges.All(me => me.Item2.Equals(m) == false))
                        {
                            // insert m into S
                            S.Add(m);
                        }
                    }
                }
            }
        }
    }
}