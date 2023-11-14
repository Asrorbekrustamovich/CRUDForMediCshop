using System.Reflection;
using System.Runtime.CompilerServices;

namespace CrudforMedicshop.Mapping
{
    public  static class ServiceMapping
    {
        public static void Addmapping(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
