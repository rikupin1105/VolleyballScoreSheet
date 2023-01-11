using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyballScoreSheet.Shared
{
    public class History
    {
        public ReactiveProperty<List<History>> Histories= new(new List<History>() { });
        public History() { }
        private History(string command1,string? command2)
        {
            Command1 = command1;
            Command2 = command2;
            DateTime = DateTime.Now;
        }
        public DateTime DateTime { get; }
        public string Command1 { get; }
        public string? Command2 { get; }

        public void HistoryAdd(string command1 , string? command2 = null)
        {
            Histories.Value.Add(new History(command1, command2));
            Histories.ForceNotify();
        }
        public void HistoryRemove()
        {
            Histories.Value.RemoveAt(Histories.Value.Count-1);
            Histories.ForceNotify();
        }
    }
}
