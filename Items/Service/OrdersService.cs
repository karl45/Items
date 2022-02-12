using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Items.Database.Repositories.IRepositories;
using Items.Models;
using Items.Service.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Service
{
    public class OrdersService:IOrdersService
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<IEnumerable<Order>> GetOrders(int page = 1,int count = 10)
        {
            try
            {
                return await _orderRepository.GetOrders(page, count);
            }
            catch
            {
                throw;
            }
        }

        public async Task AddOrder(Order order)
        {
            try
            {
                await _orderRepository.AddOrder(order);
            }
            catch
            {
                throw;
            }

        }

        public async Task<string> GenerateFile()
        {
            var orders = await _orderRepository.GetAllOrders();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var fileName = Path.Combine(path, @"Orders.xlsx");
           
            if (File.Exists(fileName))
                File.Delete(fileName);

            var file = File.Create(fileName);
            file.Close();

            using (SpreadsheetDocument xl = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                List<OpenXmlAttribute> oxa;
                OpenXmlWriter oxw;

                xl.AddWorkbookPart();
                WorksheetPart wsp = xl.WorkbookPart.AddNewPart<WorksheetPart>();

                oxw = OpenXmlWriter.Create(wsp);
                oxw.WriteStartElement(new Worksheet());
                oxw.WriteStartElement(new SheetData());
                
                oxa = new List<OpenXmlAttribute>();
                oxa.Add(new OpenXmlAttribute("r", null, "1"));

                oxw.WriteStartElement(new Row(), oxa);
                oxa = new List<OpenXmlAttribute>();


                oxa.Add(new OpenXmlAttribute("t", null, "str"));
                oxw.WriteStartElement(new Cell(), oxa);
                oxw.WriteElement(new CellValue("Id"));
                oxw.WriteEndElement();

                oxw.WriteStartElement(new Cell(), oxa);
                oxw.WriteElement(new CellValue("ItemId"));
                oxw.WriteEndElement();


                oxw.WriteStartElement(new Cell(), oxa);
                oxw.WriteElement(new CellValue("OrderDate"));
                oxw.WriteEndElement();

                oxw.WriteStartElement(new Cell(), oxa);
                oxw.WriteElement(new CellValue("RegionId"));
                oxw.WriteEndElement();

                oxw.WriteStartElement(new Cell(), oxa);
                oxw.WriteElement(new CellValue("Amount"));
                oxw.WriteEndElement();

                oxw.WriteEndElement();

                for (int i = 0; i < orders.Count; i++)
                {
                    oxa = new List<OpenXmlAttribute>();

                    oxa.Add(new OpenXmlAttribute("r", null, (i+2).ToString()));

                    oxw.WriteStartElement(new Row(), oxa);
                    oxa = new List<OpenXmlAttribute>();
                    oxa.Add(new OpenXmlAttribute("t", null, "str"));

                    oxw.WriteStartElement(new Cell(), oxa);
                    oxw.WriteElement(new CellValue(orders[i].Id));
                    oxw.WriteEndElement();

                    oxw.WriteStartElement(new Cell(), oxa);
                    oxw.WriteElement(new CellValue(orders[i].ItemId));
                    oxw.WriteEndElement();

                    oxw.WriteStartElement(new Cell(), oxa);
                    oxw.WriteElement(new CellValue(orders[i].OrderDate));
                    oxw.WriteEndElement();

                    oxw.WriteStartElement(new Cell(), oxa);
                    oxw.WriteElement(new CellValue(orders[i].RegionId));
                    oxw.WriteEndElement();

                    oxw.WriteStartElement(new Cell(), oxa);
                    oxw.WriteElement(new CellValue(orders[i].Amount));
                    oxw.WriteEndElement();

                    oxw.WriteEndElement();
                }

                oxw.WriteEndElement();
                oxw.WriteEndElement();
                oxw.Close();

                oxw = OpenXmlWriter.Create(xl.WorkbookPart);
                oxw.WriteStartElement(new Workbook());
                oxw.WriteStartElement(new Sheets());

                oxw.WriteElement(new Sheet()
                {
                    Name = "Sheet1",
                    SheetId = 1,
                    Id = xl.WorkbookPart.GetIdOfPart(wsp)
                });

                oxw.WriteEndElement();
                oxw.WriteEndElement();
                oxw.Close();

                xl.Close();
            }

            return fileName;

        }

        public async Task DeleteOrder(int id)
        {
            try
            {
                await _orderRepository.DeleteOrder(id);
            }
            catch
            {
                throw;
            }

        }

        public async Task UpdateOrder(int id, int regionId,DateTime orderDate,int itemId,int amount)
        {
            try
            {
                await _orderRepository.UpdateOrder(id, regionId, orderDate, itemId, amount);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Order>> SearchOrder(string search)
        {
            try
            {
               return await _orderRepository.SearchOrder(search);
            }
            catch
            {
                throw;
            }
        }
    }
}
