using System;
using System.Runtime.CompilerServices;

namespace MusicStore.Models.Module
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple =false)]
    internal class DBItemAttribute : Attribute
    {
        public string ItemName { get; set; }
        public DBItemAttribute([CallerMemberName] string Name = null)
        {
            ItemName = Name;
        }
    }
}