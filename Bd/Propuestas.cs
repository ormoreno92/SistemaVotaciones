//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bd
{
    using System;
    using System.Collections.Generic;
    
    public partial class Propuestas
    {
        public int propuesta_id { get; set; }
        public string propuesta_nombre { get; set; }
        public string propuesta_descripcion { get; set; }
        public int candidato_id { get; set; }
    
        public virtual Candidatos Candidatos { get; set; }
    }
}
