using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Database
{
    public sealed class Command
    {
        public string Query { get; }

        private Dictionary<string, object> _Parameters;
        public IEnumerable<KeyValuePair<string, object>> Parameters
        {
            get { return _Parameters.Select(p => p); }
        }

        public bool IsProcedure { get; }

        public Command(string Query, bool IsProcedure = false)
        {
            if(string.IsNullOrEmpty(Query))
            {
                throw new ArgumentNullException(nameof(Query));
            }

            this.Query = Query;
            this.IsProcedure = IsProcedure;
            this._Parameters = new Dictionary<string, object>();
        }

        public void AddParameter(string ParameterName, object Value)
        {
            if (string.IsNullOrEmpty(ParameterName))
            {
                throw new ArgumentNullException(nameof(ParameterName));
            }

            if(_Parameters.ContainsKey(ParameterName))
            {
                throw new ArgumentException($"The parameter {ParameterName} is already contained");
            }

            _Parameters.Add(ParameterName, Value);
        }
    }
}
