/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD_toolbox.Data.TestData
{
    public enum TestDataLevel { Bascic=0, Tricky, Exhaustive}
    /// <summary>
    /// Test data generator.
    /// I think this is used in several projects to validate odd bits of maths.
    /// The code looks poor and needs attention.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TestDataGenerator<T> : IEnumerable<T>
    {
        Cascader values;

        public TestDataGenerator(T item, TestDataLevel level)
        {
            Type type = typeof(T);
            IGetDataItems dataItems = item as IGetDataItems;
            if (dataItems != null)
            {

                //values = new Cascader(item.
            }
            else
            {
                T[] testData = GetTestData<T>(level);

                values = new Cascader(new List<IEnumerable> { testData });
            }
        }

        //public static void FillWithTestData(IGetDataItems item, int nthTest=-1)
        //{
            
        //}

        public static T[] GetTestData<T>(TestDataLevel level)
        {
            Type type = typeof(T);
            List<T> items = new List<T>();
            
            if (type == typeof(int))
            {
                var numbers = GetTestDataNumbers(level);
                var usefulNumbers = from N in numbers where N is T  select (T)N;
                items = usefulNumbers.Distinct().ToList();
            }

            if (type.GetInterfaces().Contains(typeof(IComparable<T>)))
            {
                items.Sort();
            }
            return items.ToArray();
        }

        private static object[] GetTestDataNumbers(TestDataLevel level)
        {
            List<object> numbers = new List<object> { 
                0, -1, 1, 100, -100, 1000, -1000, Math.PI, Math.PI/2, Math.PI*2,       
                decimal.MinValue, decimal.MaxValue,
                double.MinValue, double.MaxValue,
                float.MinValue, float.MaxValue,

                int.MinValue, int.MaxValue,
                long.MinValue, long.MaxValue,
                short.MinValue, short.MaxValue,
                sbyte.MinValue, sbyte.MaxValue,

                ulong.MaxValue, uint.MaxValue,
                byte.MaxValue, byte.MaxValue,
                };

            if(level > TestDataLevel.Bascic)
            {
                numbers.AddRange(getValuesOf2ToTheN());
            }
            if (level > TestDataLevel.Exhaustive)
            {
                numbers.Add(8224055000); //dosn
            }

            numbers = numbers.Distinct().ToList();
            numbers.Sort();
            return numbers.ToArray();
        }

        private static List<object> getValuesOf2ToTheN()
        {
            List<object> numbers = new List<object>();
            decimal pow = 1;
            for (int i = 0; i < 96; i++) //96 bit mantisa
            {
                numbers.Add(pow);
                numbers.Add(-pow);

                pow = pow * 2;
            }

            return numbers;
        }

        public IEnumerator<T> GetEnumerator()
        {
            //return values.AsEnumerable();
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
