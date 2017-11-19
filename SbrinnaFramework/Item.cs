// --------------------------------
// <copyright file="Item.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace SbrinnaCoreFramework
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Newtonsoft.Json;
    using SbrinnaCoreFramework.UI;

    /// <summary>
    /// Implements Item class
    /// </summary>
    public class Item
    {
        private List<UIDataHeader> headers;

        public int Code { get; set; }

        public string ItemType { get; set; }

        public string Name { get; set; }

        public string NamePlural { get; set; }

        public void AddHeader(UIDataHeader header)
        {
            if (this.headers == null)
            {
                this.headers = new List<UIDataHeader>();
            }

            this.headers.Add(header);
        }

        public ReadOnlyCollection<UIDataHeader> Headers
        {
            get
            {
                if (this.headers == null)
                {
                    this.headers = new List<UIDataHeader>();
                }

                return new ReadOnlyCollection<UIDataHeader>(this.headers);
            }
        }

        public UIButton NewButton { get; set; }

        public string ViewLink { get; set; }

        public Item()
        {
        }

        public static Item GetByCode(int code, Dictionary<string,string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }
            
            Item deserializedProduct = new Item();
            string path = HttpContext.Current.Request.PhysicalApplicationPath + "ItemDefinition";
            if(!path.EndsWith(@"\", StringComparison.Ordinal))
            {
                path += @"\";
            }

            var myFiles = Directory.GetFiles(path, string.Format(CultureInfo.CurrentCulture, "{0}-*.*", code), SearchOption.TopDirectoryOnly).ToList();

            if (myFiles.Count > 0)
            {
                string filename = myFiles[0];
                using (StreamReader input = new StreamReader(filename))
                {
                    deserializedProduct = JsonConvert.DeserializeObject<Item>(input.ReadToEnd());

                    // Translation
                    deserializedProduct.Name = dictionary[deserializedProduct.Name];
                    deserializedProduct.NamePlural = dictionary[deserializedProduct.NamePlural];
                    deserializedProduct.NewButton.Text = dictionary[deserializedProduct.NewButton.Text];
                }
            }

            return deserializedProduct;
        }
    }
}
