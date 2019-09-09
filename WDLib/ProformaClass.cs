/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox.AplicationFramework;
using WD_toolbox.Data;
using WD_toolbox.Data.DataStructures;
using WD_toolbox.Maths.Range;
using WD_toolbox.Validation;

namespace WD_toolbox
{
    /// <summary>
    /// This class is an example of the WDLib coding practice
    /// </summary>
    [Serializable()]
    public class ProformaClass : IEquatable<ProformaClass>, IValidatable, ICloneable, IComparable<ProformaClass>, IGetDataItems
                                 //,IDeserializationCallback, IFormattable
    {
        //-----------------------------------------------------------------------------------------------
        // Static Data
        //-----------------------------------------------------------------------------------------------
        public static int MaxAge { get { return 128; } }
        public static Version Version { get { return new Version("1.0"); } }

        //-----------------------------------------------------------------------------------------------
        // Instance Data
        //-----------------------------------------------------------------------------------------------
        public string Name { get; protected set; }

        int _age;
        public int Age
        {
            get { return _age; }
            protected set { _age = Range.clamp(value, 0, MaxAge); }
        }

        //-----------------------------------------------------------------------------------------------
        // Constructors and factory methods
        //-----------------------------------------------------------------------------------------------
        public ProformaClass() : this ("un-named", 0)
        {

        }

        protected ProformaClass(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }

        protected ProformaClass(ProformaClass another)
        {
            this.Name = another.Name;
            this.Age = another.Age;
        }

        public static ProformaClass FromNameAndAge(string name, int age)
        {
            ProformaClass p = new ProformaClass(name, age);
            Why valid = p.Valid();
            if (valid)
            {
                return p;
            }
            else
            {
                WDAppLog.logError(ErrorLevel.Error, "Invalid data: " + valid.Reason);
                return null;
            }
        }

        //----------------------------------------------------------------------------------------
        // Serialisation
        //----------------------------------------------------------------------------------------
        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            
        }
        /*
        public byte[] SaveToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(Version.ToString());
                bw.Write((Int32) this.Age);
                bw.Write(this.Name);
                return ms.ToArray();
            }
        }

        public static ProformaClass LoadFromBytes(byte[] data)
        {
            using (Stream inputStream = new MemoryStream(data))
            using (BinaryReader br = new BinaryReader(inputStream, Encoding.Unicode, true))
            {
                Version version = new Version(br.ReadString());

                if (version > ProformaClass.Version)
                {
                    WDAppLog.logError(ErrorLevel.Error, "Incompatable version number.");
                    return null;
                }

                int age = br.ReadInt32();
                string name = br.ReadString();

                ProformaClass p = ProformaClass.FromNameAndAge(name, age);
                return p;
            }
        }

        public string ToParsableString()
        {
            //return System.Convert.ToBase64String(SaveToBytes());
        }

        public override string ToString()
        {
            return string.fo
        }

        public bool TryParse(string text)
        {

        }
        */
        //-----------------------------------------------------------------------------------------------
        // IEquatable<ProformaClass>
        //-----------------------------------------------------------------------------------------------
        public override bool Equals(object obj)
        {
            return (obj is ProformaClass) ? (this == (ProformaClass)obj) : false;
        }

        public bool Equals(ProformaClass rec)
        {
            return this == rec;
        }

        public static bool operator ==(ProformaClass a, ProformaClass b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            //If not IComparable<> do this
            //return (a.Age == b.Age) && (a.Name == b.Name);

            //If IComparable<>, do this so that no conflict emerges
            return a.CompareTo(b) == 0;
        }

        public static bool operator !=(ProformaClass a, ProformaClass b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Misc.HashItems(Age, Name);
        }

        //-----------------------------------------------------------------------------------------------
        // IComparable<>
        //-----------------------------------------------------------------------------------------------
        public int CompareTo(ProformaClass other)
        {
            //baisly this is (other != nuul), except it safe gards against a infinite recursion possible
            //when overloaded equalty operators that call CompareTo
            if(object.ReferenceEquals(other, null)) 
            {
                //By definition, any object compares greater than (or follows) Nothing, and two null references compare equal to each other.
                return 1;
            }

            int compare = 0;

            //compare name
            compare = Name.CompareTo(other.Name);
            if (compare != 0)
            {
                return compare;
            }

            //compare age
            compare = Age.CompareTo(other.Age);
            if (compare != 0)
            {
                return compare;
            }


            //must be equal
            return 0;
        }

        //-----------------------------------------------------------------------------------------------
        // IValidatable
        //-----------------------------------------------------------------------------------------------
        public Why Valid()
        {
            Why valid = true;

            if (Age > MaxAge)
            {
                valid &= Why.FalseBecause("Age can not be greater than {0}.", MaxAge);
            }
            if (Age < 0)
            {
                valid &= Why.FalseBecause("Age can not be negative.");
            }
            if (Name == null)
            {
                valid &= Why.FalseBecause("Name can not be null.");
            }

            return valid;
        }

        public object Clone()
        {
            return new ProformaClass(this);
        }

        //-----------------------------------------------------------------------------------------------
        // IGetDataItems
        //-----------------------------------------------------------------------------------------------
        protected static readonly string[] DataNames = { "Name", "Age" };
        string[] IGetDataItems.DataNames
        {
            get { return DataNames; }
        }
    }
}
