 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.Core;
using System.Configuration;
using MongoCURD.App_Start;
using MongoDB.Driver;
using MongoCURD.Models;

namespace MongoCURD.Controllers
{
    public class ProductController : Controller
    {
        private MongoDBContext dBContext;
        private IMongoCollection<ProductModel> productCollection;

        public ProductController()
        {
            dBContext = new MongoDBContext();
            productCollection = dBContext.database.GetCollection<ProductModel>("productmanagementdb");
        }
        // GET: Product
        public ActionResult Index()
        {
            List<ProductModel> product = productCollection.AsQueryable<ProductModel>().ToList();
             
            return View(product);
        }

        // GET: Product/Details/5
        public ActionResult Details(string id)
        {
            var productId = new ObjectId(id);
            var product = productCollection.AsQueryable<ProductModel>().SingleOrDefault(x => x.Id == productId);
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(ProductModel product)
        {
            try
            {
                productCollection.InsertOne(product);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(string id)
        {
            var productId = new ObjectId(id);
            var product = productCollection.AsQueryable<ProductModel>().SingleOrDefault(x => x.Id == productId);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(string  id, ProductModel product)
        {
            try
            {
                var filter = Builders<ProductModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<ProductModel>.Update.Set("ProductName", product.ProductName).Set("ProductDescription", product.ProductDescription).Set("Quantity", product.Quantity);
                var result = productCollection.UpdateOne(filter,update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(string id)
        {
            var productId = new ObjectId(id);
            var product = productCollection.AsQueryable<ProductModel>().SingleOrDefault(x => x.Id == productId);
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                productCollection.DeleteOne(Builders<ProductModel>.Filter.Eq("_id", ObjectId.Parse(id)));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
