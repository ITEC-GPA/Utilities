using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPC.Utilities.Attributes
{
    public sealed class UIAttribute : Attribute
    {
        private string _description;
        private string _group;
        private string _kind;

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public string Group
        {
            get => _group;
            set => _group = value;
        }

        public string Kind
        {
            get => _kind;
            set => _kind = value;
        }

        public UIAttribute()
        {
            _description = "";
            _group = "";
            _kind = "";
        }
    }
}
