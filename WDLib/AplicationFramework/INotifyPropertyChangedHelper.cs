/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WD_toolbox.AplicationFramework
{
// Example
/*
 * 
    INotifyPropertyChangedHelper notifyHelper = new INotifyPropertyChangedHelper();
    private string foo;
    public string Foo
    {
        set { notifyHelper.SetField(ref foo, value); }
    }
     
    protected void propertyChanged(bool resultsInNewMaze = false, [CallerMemberName] string propertyName = null)
    {
        if (resultsInNewMaze)
        {
            Generate(Seed);
        }
        if (notifyHelper == null)
        {
            notifyHelper = new INotifyPropertyChangedHelper();
            notifyHelper.PropertyChanged += (S, E) => {if(this.PropertyChanged != null) { this.PropertyChanged(S, E);}};
        }
        notifyHelper.Raise(propertyName);
    } 
            
*/


    /// <summary>
    /// Based on some ideas at http://stackoverflow.com/questions/1315621/implementing-inotifypropertychanged-does-a-better-way-exist
    /// </summary>
    public sealed class INotifyPropertyChangedHelper : INotifyPropertyChanged
    {
        //not going to work
        /*public INotifyPropertyChangedHelper(INotifyPropertyChanged what)
        {
            this.PropertyChanged += (S, E) => { if (what.PropertyChanged != null) { what.PropertyChanged(S, E); } };
        }*/

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            //simple check, but I am not a fan, I dont trust people to implement IEquatable correctly, for the purposes of property grids.
            //In the edge case this causes an issue, the bug could be very hard to find.
            //I can live with the odd redundent update.
            /*if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }*/

            field = value;
            OnPropertyChanged(propertyName);
        }

        public void Raise([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(propertyName);
        }
    }

}
