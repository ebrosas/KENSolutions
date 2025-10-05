using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class LabelAttribute : Attribute
    {
        public string Text { get; }
        public LabelAttribute(string text) => Text = text;
    }
}
