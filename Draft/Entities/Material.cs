//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Draft.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Material
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material()
        {
            this.MaterialCountHistory = new HashSet<MaterialCountHistory>();
            this.MaterialSupplier = new HashSet<MaterialSupplier>();
            this.ProductMaterial = new HashSet<ProductMaterial>();
        }
    
        public int ID { get; set; }
        public string Title { get; set; }
        public int CountInPack { get; set; }
        public string Unit { get; set; }
        public Nullable<double> CountInStock { get; set; }
        public double MinCount { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string Image { get; set; }
        public string CheckImage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Image))
                    return null;
                else
                    return Image;
            }
        }
        public int MaterialTypeID { get; set; }
        public string Supplier
        {
            get
            {
                string supplier = null;

                foreach (var item in MaterialSupplier)
                {
                    supplier += item.Supplier.Title + ", ";
                }

                if (supplier != null)
                    return supplier.Remove(supplier.Length - 2, 2);
                else
                    return "Не имеются";
            }
        }
        public int BackgroundCount
        {
            get
            {
                if (CountInStock < MinCount)
                    return 1;
                else if (CountInStock >= MinCount * 3)
                    return 2;
                else
                    return 3;
            }
        }

        public virtual MaterialType MaterialType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialCountHistory> MaterialCountHistory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialSupplier> MaterialSupplier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductMaterial> ProductMaterial { get; set; }
    }
}
