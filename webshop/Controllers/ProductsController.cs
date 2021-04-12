﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webshop;
using webshop.ViewModel;

namespace webshop.Controllers
{
    public class ProductsController : Controller
    {
        private webshopEntities db = new webshopEntities();

        // GET: Products
        public ActionResult Index()
        {
            var product = db.Product.Include(p => p.Category).Include(p => p.Manufacturer);
            return View(product.ToList());
        }

        public ActionResult Product(string searchString, string currentFilter, bool? filterM, bool? filterC)
        {
            // standard product list
            var product = db.Product.Include(p => p.Category).Include(p => p.Manufacturer);

            //Search box
            if (!String.IsNullOrEmpty(searchString))
            {
                product = db.Product
                    .Include(p => p.Category)
                    .Include(p => p.Manufacturer)
                    .Where(p => p.Product_Name.Contains(searchString)
                                    || p.Category.Category_Name.Contains(searchString)
                                    || p.Manufacturer.Manufacturer_Name.Contains(searchString));

                // When category is checked and Manufacturer unchecked
                if (filterM == false & filterC == true)
                {
                    product = db.Product
                        .Include(p => p.Category)
                        .Where(p => p.Category.Category_Name.Contains(searchString));
                }
                // When Manufacturer is checked and category unchecked
                if (filterM == true & filterC == false)
                {
                    product = db.Product
                        .Include(p => p.Category)
                        .Where(p => p.Manufacturer.Manufacturer_Name.Contains(searchString));
                }

            }


            return View(product.ToList());
        }

 
        //public ActionResult OrderLineIndexVM()
        //{

         
       

        //    var orderline = db.OrderLine.Where(x => x.Order_ID).FirstOrDefault();

        //    var productlist = db.Product.Include(p => p.Category).Include(p => p.Manufacturer).ToList();

        //    foreach (var product in productlist)
        //    {
        //        var temp_line = new OrderLineProductViewModel()
        //        {

        //        };


        //    }


        //    return View();
        //}

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Category_ID = new SelectList(db.Category, "Category_ID", "Category_Name");
            ViewBag.Manufacturer_ID = new SelectList(db.Manufacturer, "Manufacturer_ID", "Manufacturer_Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_ID,Manufacturer_ID,Category_ID,Product_Name,NetUnitPrice,ImagePath,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_ID = new SelectList(db.Category, "Category_ID", "Category_Name", product.Category_ID);
            ViewBag.Manufacturer_ID = new SelectList(db.Manufacturer, "Manufacturer_ID", "Manufacturer_Name", product.Manufacturer_ID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_ID = new SelectList(db.Category, "Category_ID", "Category_Name", product.Category_ID);
            ViewBag.Manufacturer_ID = new SelectList(db.Manufacturer, "Manufacturer_ID", "Manufacturer_Name", product.Manufacturer_ID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product_ID,Manufacturer_ID,Category_ID,Product_Name,NetUnitPrice,ImagePath,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_ID = new SelectList(db.Category, "Category_ID", "Category_Name", product.Category_ID);
            ViewBag.Manufacturer_ID = new SelectList(db.Manufacturer, "Manufacturer_ID", "Manufacturer_Name", product.Manufacturer_ID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
