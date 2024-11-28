namespace PetHome.Domain.GeneralValueObjects
{
    public record RequisitesDetails 
    {
        public IReadOnlyList<Requisites> Values { get; }
         
        private RequisitesDetails() { }
        public RequisitesDetails(IReadOnlyList<Requisites> values)
        {
            Values = values;
        }

        public static RequisitesDetails Create(List<Requisites> values)
        {
            return new RequisitesDetails(values);
        }
    }
}