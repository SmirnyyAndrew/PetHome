using System.Reflection;

namespace PetHome.Accounts.Contracts;
public static class AssemblyReference
{
    public static Assembly Assembly => typeof(Assembly).Assembly;
}
