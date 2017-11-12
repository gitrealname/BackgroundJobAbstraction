using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLtss.Infrastructure.Customization.BackgroundJob;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.TypeRules;
using System.Linq.Expressions;

namespace HangFirePrototype
{
    public class StructureMapBackgroundJobTypeScannerConvention : ConfigurableRegistrationConvention
    {
        public static readonly HashSet<Type> RecurringJobTypes = new HashSet<Type>();

        public StructureMapBackgroundJobTypeScannerConvention()
        {
        }
        public override void ScanTypes(TypeSet types, Registry registry)
        {
            var typeCollection = types.FindTypes(TypeClassification.Concretes)
                .Where(type => type.IsConcreteAndAssignableTo(typeof(IRecurringBackgroundJob)));

            foreach(var t in typeCollection)
            {
                RecurringJobTypes.Add(t);
            }
        }

        public override string ToString()
        {
            return "IRecurringBackgroundJob registration convention";
        }
    }
}
