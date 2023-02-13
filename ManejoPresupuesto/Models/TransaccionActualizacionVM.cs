namespace ManejoPresupuesto.Models
{
    public class TransaccionActualizacionVM : TransaccionVM
    {
        public int CuentaAnterioId { get; set; }
        public decimal MontoAnterio { get; set; }


    }
}
