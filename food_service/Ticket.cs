using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Implementation;
using System.Data;

namespace food_service
{
    class Ticket:IDisposable
    {
        RegistroImpl registroImpl;
        ClienteImpl clienteImpl;
        SnackImpl snackImpl;
        ItemImpl itemImpl;
        OrdenImpl ordenImpl;

        DataTable dtRegistro;
        DataTable dtCliente;
        DataTable dtItem;
        DataTable dtSnack;
        DataTable dtOrden;

        Tickets.TicketsComedor ticketImprimir;
        Tickets.TicketLunch ticketLunch;
        Tickets.TicketSnack ticketSnack;


        public void ImprimirTicketRegistro(int idRegistro, int codigo)
        {
            try
            {

                registroImpl = new RegistroImpl();
                clienteImpl = new ClienteImpl();
                dtRegistro = new DataTable();
                dtCliente = new DataTable();

                dtRegistro = registroImpl.GetTableRegistro(idRegistro);
                dtCliente = clienteImpl.GetTableCliente(codigo);

                ticketImprimir = new Tickets.TicketsComedor();
                ticketImprimir.Database.Tables["registro"].SetDataSource(dtRegistro);
                ticketImprimir.Database.Tables["cliente"].SetDataSource(dtCliente);

                ticketImprimir.PrintToPrinter(1, true, 0, 0);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                ticketImprimir.Dispose();
            }
        }
        public void ImprimirTicketLunch(int idSnack, int codigo)
        {
            try
            {
                dtCliente = new DataTable();
                dtItem = new DataTable();
                dtSnack = new DataTable();

                snackImpl = new SnackImpl();
                clienteImpl = new ClienteImpl();
                itemImpl = new ItemImpl();

                dtSnack = snackImpl.GetTableSnack(idSnack);
                dtCliente = clienteImpl.GetTableCliente(codigo);
                dtItem = itemImpl.SelectDataTableItem(int.Parse(dtSnack.Rows[0][3].ToString()));


                ticketLunch = new Tickets.TicketLunch();
                ticketLunch.Database.Tables["snack"].SetDataSource(dtSnack);
                ticketLunch.Database.Tables["cliente"].SetDataSource(dtCliente);
                ticketLunch.Database.Tables["item"].SetDataSource(dtItem);
                ticketLunch.PrintToPrinter(1, true, 0, 0);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ticketLunch.Dispose();
            }

        }
        public void ImprimirTicketSnack(int idOrden, int codigo)
        {
            try
            {
                dtCliente = new DataTable();
                dtItem = new DataTable();
                dtSnack = new DataTable();
                dtOrden = new DataTable();

                snackImpl = new SnackImpl();
                clienteImpl = new ClienteImpl();
                itemImpl = new ItemImpl();
                ordenImpl = new OrdenImpl();

                dtSnack = snackImpl.GetTableSnackWhitOrderID(idOrden);
                dtCliente = clienteImpl.GetTableCliente(codigo);
                dtOrden = ordenImpl.GetOrdenForId(idOrden);
                dtItem = itemImpl.SelectItemsVentaSnack(int.Parse(dtOrden.Rows[0][0].ToString()));


                ticketSnack = new Tickets.TicketSnack();
                ticketSnack.Database.Tables["snack"].SetDataSource(dtSnack);
                ticketSnack.Database.Tables["cliente"].SetDataSource(dtCliente);
                ticketSnack.Database.Tables["orden"].SetDataSource(dtOrden);
                ticketSnack.Database.Tables["item"].SetDataSource(dtItem);


                ticketSnack.PrintToPrinter(1, true, 1, 1);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ticketSnack.Dispose();
            }

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
