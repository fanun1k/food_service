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
    class Ticket
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

        int cantidad_productos_snack;
        string nombre_tamaño_pagina;
        public void ImprimirTicketRegistro(int id, int codigo)
        {
            try
            {

                registroImpl = new RegistroImpl();
                clienteImpl = new ClienteImpl();
                dtRegistro = new DataTable();
                dtCliente = new DataTable();

                dtRegistro = registroImpl.GetTableRegistro(id);
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
        public void ImprimirTicketSnack(int idSnack, int codigo)
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

                dtSnack = snackImpl.GetTableSnack(idSnack);
                dtCliente = clienteImpl.GetTableCliente(codigo);
                dtItem = itemImpl.SelectDataTableItem(int.Parse(dtSnack.Rows[0][3].ToString()));
                dtOrden = ordenImpl.GetOrdenForId(int.Parse(dtSnack.Rows[0][11].ToString()));


                ticketSnack = new Tickets.TicketSnack();
                ticketSnack.Database.Tables["snack"].SetDataSource(dtSnack);
                ticketSnack.Database.Tables["cliente"].SetDataSource(dtCliente);
                ticketSnack.Database.Tables["item"].SetDataSource(dtItem);
                ticketSnack.Database.Tables["orden"].SetDataSource(dtOrden);

                //probar esto para tamaños personalizados
                System.Drawing.Printing.PrintDocument doctoprint = new System.Drawing.Printing.PrintDocument();
                doctoprint.PrinterSettings.PrinterName = "EPSON TM-T81 Receipt"; //'(ex. "Epson SQ-1170 ESC/P 2")

                cantidad_productos_snack = dtSnack.Rows.Count;
                switch (cantidad_productos_snack)
                {
                    case 1:
                    case 2:
                    case 3:
                        nombre_tamaño_pagina = "valeSnack 1a3";
                        break;
                    case 4:
                    case 5:
                    case 6:
                        nombre_tamaño_pagina = "valeSnack 4a6";
                        break;
                    case 7:
                    case 8:
                    case 9:
                        nombre_tamaño_pagina = "valeSnack 7a9";
                        break;
                }

                for (int i = 0; i < doctoprint.PrinterSettings.PaperSizes.Count - 1; i++)
                {
                    int rawKind;
                    if (doctoprint.PrinterSettings.PaperSizes[i].PaperName == nombre_tamaño_pagina)
                    {
                        rawKind = Convert.ToInt32(doctoprint.PrinterSettings.PaperSizes[i].GetType().GetField("kind", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(doctoprint.PrinterSettings.PaperSizes[i]));
                        //crPrintOut.PrintOptions.PaperSize = rawKind;
                        ticketSnack.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind;
                        break;
                    }
                }
                //fin

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

    }
}
