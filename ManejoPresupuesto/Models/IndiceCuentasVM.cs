namespace ManejoPresupuesto.Models
{
    public class IndiceCuentasVM
    {
        public string? TipoCuenta { get; set; }
        public IEnumerable<Cuentas>? Cuenta { get; set; }
        public decimal Balance => Cuenta.Sum(x => x.Balance);
    }
}
