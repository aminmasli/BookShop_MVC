﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shop.DataAccess.Repository.IRepository;
using Shop.Models;
using Shop.Models.ViewModels;

namespace BookShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork ,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();

            return View(objProductList);
        }
        public IActionResult Upsert(int? id) 
        {

            ProductVm productVm = new()
            {
                CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
            Product = new Product()


         };
            if(id == null || id == 0)
            {
                // Create
                return View(productVm);
            }
            else
            {
                //update
                productVm.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVm);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVm productVm, IFormFile? file)
        {
            
            {

                if (ModelState.IsValid)

                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if(file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);    
                        string productPath = Path.Combine(wwwRootPath, @"images\product");

                        if(string.IsNullOrEmpty(productVm.Product.ImageUrl))
                        {
                            // delete the old image
                            var oldImagePath = 
                                Path.Combine(wwwRootPath, productVm.Product.ImageUrl.TrimStart('\\'));

							if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        productVm.Product.ImageUrl = @"\images\product\" + fileName;
                    }
                    if(productVm.Product.Id == 0)
                    {
                        _unitOfWork.Product.Add(productVm.Product);

                    }
                    else
                    {
                        _unitOfWork.Product.Update(productVm.Product);
                    }
                  
                    _unitOfWork.Save();
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction("Index");
                }
                else
                {
					productVm.CategoryList = _unitOfWork.Category
                    .GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });


			        return View(productVm);
				}
				
            } 
         
        }


    #region API CALLS
    [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDelete = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath =
                                Path.Combine(_webHostEnvironment.WebRootPath,
                                productToBeDelete.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(productToBeDelete);
            _unitOfWork.Save();  
            return Json(new { success = true,message = "Delete Successful" });
        }

        #endregion
    }
}
