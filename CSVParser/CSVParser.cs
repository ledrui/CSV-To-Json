using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace CSVParser
{
    public class Item 
    {
        public string sku { get; set; }
        public int qty { get; set; }
    }

    public class Buyer 
    {
        public string name { get; set; }
        public string street { get; set; }
        public string zip { get; set; }
    }

    public class Timing 
    {
        public int start { get; set; }
        public int stop { get; set; }
        public int gap { get; set; }
        public int offset { get; set; }
        public int pause { get; set; }


    }

    public class Order 
    {
        public string date { get; set; }
        public string code { get; set; }
        public string number { get; set; }
        public Buyer buyer { get; set; }
        public List<Item> items { get; set; }
        public Timing timing { get; set; }

    }

    public class Ender 
    {
        public int process { get; set;}
        public int paid { get; set; }
        public int created { get; set; }
    }


    public class FileDict
    {
        public string date { get; set; }
        public string type { get; set; }

        public IList<Order> orders { get; set; }
        public Ender ender { get; set; }
        //    public Dictionary<string, int> ender { get; set; }
    }

    public class CSVParser
    {  
        FileDict _file;
        public CSVParser()
        {
            _file = new FileDict();
        }
        // public FileDict file = new FileDict();
        public Dictionary<string, int> ender;
        
        public string Parse(string path)
        {

            try 
            {
                using(StreamReader sr = new StreamReader(path))
                {
                    string line;

                    while((line = sr.ReadLine()) != null)
                    {   
                        string[] row  = line.Split(",");
                        
                        
                        string first = row[0].Trim('"');
                        if (first.IndexOf('F') != -1) {
                            if (row.Length >= 2) {
                                _file.date = row[1].Trim('"');
                            }
                            if (row.Length >=3) {
                                _file.type = row[2].Trim('"');
                            }
                            
                        }
                        if (first.IndexOf('O') != -1) {
                            Order order = new Order();
                            order.date = row[1].Trim('"');
                            order.code = row[2].Trim('"');
                            order.number = row[3].Trim('"');
                            
                            if (_file.orders == null){
                                _file.orders = new List<Order>();
                            }
                            
                            _file.orders.Add(order);
                        }
                        if (first.IndexOf('B') != -1) {
                        
                            if (_file.orders != null){
                                Buyer buyer = new Buyer();
                                buyer.name = row[1].Trim('"');
                                buyer.street = row[2].Trim('"');
                                buyer.zip = row[3].Trim('"');
                                Order last_order = _file.orders[_file.orders.Count - 1];
                                last_order.buyer = buyer;
                                _file.orders[_file.orders.Count - 1] = last_order;
                            }
                        }
                        else if (first.IndexOf('T') != -1) {
                            
                            if (_file.orders != null){
                                Timing time = new Timing();
                                time.start = int.Parse(row[1].Trim('"'));
                                time.stop = int.Parse(row[2].Trim('"'));
                                time.offset = int.Parse(row[3].Trim('"'));
                                time.pause = int.Parse(row[4].Trim('"'));
                                Order last_order = _file.orders[_file.orders.Count - 1];
                                last_order.timing = time;
                                _file.orders[_file.orders.Count - 1] = last_order;
                            }
                        }
                        else if (first.IndexOf('L') != -1) {
                            if (_file.orders != null){
                                Item item = new Item();
                                item.sku = row[1].Trim('"');
                                item.qty = int.Parse(row[2].Trim('"'));
                                
                                Order last_order = _file.orders[_file.orders.Count - 1];
                                if (last_order.items == null){
                                    last_order.items = new List<Item>();
                                }
                                last_order.items.Add(item);
                                _file.orders[_file.orders.Count - 1] = last_order;
                            }
                        }
                        else if (first.IndexOf('E') != -1) {
                            // ender = new Dictionary<string, int>();
                            // ender.Add("proccess", int.Parse(row[1].Trim('"')) );
                            // ender.Add("paid", int.Parse(row[2].Trim('"')));
                            // ender.Add("created", int.Parse(row[3].Trim('"')));
                            // _file.ender = ender;
                            Ender end = new Ender();
                            end.process = int.Parse(row[1].Trim('"'));
                            end.paid = int.Parse(row[2].Trim('"'));
                            end.created = int.Parse(row[3].Trim('"'));
                            _file.ender = end;
                        }
                        
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return JsonSerializer.Serialize(_file);
        }

    }
}
