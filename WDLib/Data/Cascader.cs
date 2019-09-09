//NB: no header, I'm not sure if this is my code.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD_toolbox.Data
{
    /// <summary>
    /// This looks like code I adapted from somewhere (needs a reference.). Possibly from some c++ stuff.
    /// I think it has to do with permutations.
    /// Marking as obsolete.
    /// </summary>
    [Obsolete]
    public class Cascader : IEnumerable<object[]>
    {
        List<IEnumerable> vectors;
        public Cascader(List<IEnumerable> vectorList)
        {
            this.vectors = vectorList;
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            List<IEnumerator> eArray = (from E in vectors select E.GetEnumerator()).ToList();
            eArray.ForEach(E => E.MoveNext());

            object[] r = new object[vectors.Count];
            for(int i=0; i<r.Length; i++)
            {
                //eArray[i].MoveNext();
                r[i] = eArray[i].Current;
            }

            bool done = false;

            while (!done)
            {
                r[0] = eArray[0].Current;
                yield return r;

                int index = 0;
                while (!eArray[index].MoveNext())
                {
                    eArray[index].Reset();
                    eArray[index].MoveNext();
                    r[index] = eArray[index].Current;
                    index++;
                    if (index >= eArray.Count)
                    {
                        done = true;
                        break;
                    }
                    r[index] = eArray[index].Current;
                }
                if (!done)
                {
                    r[index] = eArray[index].Current;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    [Obsolete]
    public class Cascader<T> : IEnumerable<T[]>
    {
        List<IEnumerable<T>> vectors;
        public Cascader(List<IEnumerable<T>> vectorList)
        {
            this.vectors = vectorList;
        }

        public IEnumerator<T[]> GetEnumerator()
        {
            List<IEnumerator<T>> eArray = (from E in vectors select E.GetEnumerator()).ToList();
            eArray.ForEach(E => E.MoveNext());

            T[] r = new T[vectors.Count];
            for (int i = 0; i < r.Length; i++)
            {
                //eArray[i].MoveNext();
                r[i] = eArray[i].Current;
            }

            bool done = false;

            while (!done)
            {
                r[0] = eArray[0].Current;
                yield return r;

                int index = 0;
                while (!eArray[index].MoveNext())
                {
                    eArray[index].Reset();
                    eArray[index].MoveNext();
                    r[index] = eArray[index].Current;
                    index++;
                    if (index >= eArray.Count)
                    {
                        done = true;
                        break;
                    }
                    r[index] = eArray[index].Current;
                }
                if (!done)
                {
                    r[index] = eArray[index].Current;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
