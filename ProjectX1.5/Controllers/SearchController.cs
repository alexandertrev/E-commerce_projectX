using ProjectX1._5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ProjectX1._5.Controllers
{
    public class SearchController : Controller
    {
        private DBmodels db = new DBmodels();
        // GET: Search
        public ActionResult AdvancedSearch()
        {
            var model = new Search
            {
                Contries = new SelectList(db.Country, "countryName", "countryName"),
                Brands = new SelectList(db.CigarBrands, "id", "brandName"),
                strength = new SelectList(db.CigarStrengths, "id", "strengthName"),
                AlcoholCategories = new SelectList(db.AlcoholCategory, "id", "categoryName"),
                Wrappers = new SelectList(db.CigaresWrapper, "wrapperName", "wrapperName")
            };

            return View(model);
        }

        public async Task<ActionResult> searchData(string fromMenu)
        {
            IList<Product> alcoholProductList = new List<Product>();
            IList<Product> cigarProductList = new List<Product>();
            IList<Bundle> BundleList = new List<Bundle>();
            IList<SearchItem> resultList = new List<SearchItem>();
            IList<SearchItem> returnList = new List<SearchItem>();

            string searchItem = Request.Form["searchItem"];
            string minPrice = Request.Form["minPrice"];
            string maxPrice = Request.Form["maxPrice"];
            string categoryID = Request.Form["categoryID"];
            string alcoholOrigin = Request.Form["alcoholOrigin"];
            string cigarBrand = Request.Form["cigarBrand"];
            string wrapperType = Request.Form["wrapperType"];
            string strengthID = Request.Form["strengthID"];
            string cigarOrigin = Request.Form["cigarOrigin"];
            string pageNumber = Request.Form["pageNumber"];
            string choices = Request.Form["choice"];

            if (fromMenu != null)
            {
                alcoholProductList = await searchAlcoholProducts(fromMenu, 0, 10000, "", "");
                cigarProductList = await searchCigarProducts(fromMenu, 0, 10000, "", "", "", "");
                await convertToSearchItemList(resultList, "alcohol", alcoholProductList, null);
                await convertToSearchItemList(resultList, "cigar", cigarProductList, null);

                return View("Search", resultList);
            }

            int pageNum = int.Parse(pageNumber);

            if (pageNum == 1)
                Thread.Sleep(2000);
            else
                Thread.Sleep(1000);

            if(choices!=null && choices.Contains("Alcohol"))
            {
                alcoholProductList = await searchAlcoholProducts(searchItem, int.Parse(minPrice), int.Parse(maxPrice), categoryID, alcoholOrigin);
                await convertToSearchItemList(resultList, "alcohol", alcoholProductList, null);
            }

            if(choices != null && choices.Contains("Cigar"))
            {
                cigarProductList = await searchCigarProducts(searchItem, int.Parse(minPrice), int.Parse(maxPrice), cigarBrand, wrapperType, strengthID, cigarOrigin);
                await convertToSearchItemList(resultList,"cigar", cigarProductList,null);
            }

            if(choices != null && choices.Contains("Bundle"))
            {
                alcoholProductList = await searchAlcoholProducts(searchItem, int.Parse(minPrice), int.Parse(maxPrice), categoryID, alcoholOrigin);
                cigarProductList = await searchCigarProducts(searchItem, int.Parse(minPrice), int.Parse(maxPrice), cigarBrand, wrapperType, strengthID, cigarOrigin);
                BundleList = await searchBundles(alcoholProductList, cigarProductList);
                await convertToSearchItemList(resultList, "bundle", null, BundleList);
            }
            returnList = resultList.Take(pageNum*6).ToList();


            string more = "";

            if (returnList.Count == resultList.Count)
                more = "no";
            else more = "yes";

            var ret = new {pList = returnList, more=more };
            JavaScriptSerializer myJSS = new JavaScriptSerializer();

            return Json(myJSS.Serialize(ret), JsonRequestBehavior.AllowGet);
        }
        public async Task<SearchItem> convertToSearchItem(Product product, Bundle bundle,Boolean isBundle, string productType, int quantity)
        {
            SearchItem result = new SearchItem();
            if(isBundle)
            {
                result.listInBundle = new List<SearchItem>();
                result.productID = bundle.bundleId;
                result.pName = bundle.bundleName;
                result.price = bundle.price;
                result.info = bundle.info;
                result.pic = bundle.pic;
                result.pType = "bundle";

                foreach (BundleInfo p in bundle.BundleInfos)
                {
                    SearchItem tempProduct;

                    if (p.Product.AlcoholProduct!=null)
                    {
                        tempProduct = await convertToSearchItem(p.Product, null, false, "alcohol", p.quantity);
                    }
                    else
                    {
                        tempProduct = await convertToSearchItem(p.Product, null, false, "cigar", p.quantity);
                    }

                    result.listInBundle.Add(tempProduct);
                }
            }
            else
            {
                result.productID = product.productID;
                result.pName = product.productName;
                result.price = product.price;
                result.info = product.info;
                result.pic = product.pic;
                result.quantity = quantity;

                if (productType.Equals("cigar"))
                {
                    result.pType = "cigar";
                    result.cigarWrapper = product.CigaresProduct.CigaresWrapper.wrapperName;
                    result.cigarBrand = product.CigaresProduct.CigarBrand1.brandName;
                    result.cigarStrength = product.CigaresProduct.CigarStrength.strengthName;
                    result.cigarBundle = product.CigaresProduct.bundle;
                    result.country = product.CigaresProduct.origin;
                }
                else
                {
                    result.pType = "alcohol";
                    result.alcoholType = product.AlcoholProduct.AlcoholCategory.categoryName;
                    result.alcoholVolume = product.AlcoholProduct.volume;
                    result.country = product.AlcoholProduct.origin;
                }
            }


            return result;
        }
        public async Task<IList<SearchItem>> convertToSearchItemList(IList<SearchItem> resultList, string pType, IList<Product> pList, IList<Bundle> bList)
        {
            if (!pType.Equals("bundle"))
            {
                foreach (Product p in pList)
                {
                    SearchItem temp = await convertToSearchItem(p, null, false, pType, 1);
                    resultList.Add(temp);
                }
            }
            else
            {
                foreach (Bundle b in bList)
                {
                    SearchItem temp = await convertToSearchItem(null, b, true, pType, 1);
                    resultList.Add(temp);
                }
            }
            
            return resultList;
        }

        public async Task<IList<Product>> searchAlcoholProducts(string searchItem, int minPrice, int maxPrice, string categoryID, string country)
        {
            IList<Product> result = null;
            int catNum = 0;

            if (!categoryID.Equals(""))
                catNum = int.Parse(categoryID);

            if (!categoryID.Equals(""))
            {
                result = await (from x in db.Product
                                where x.price >= minPrice && x.price <= maxPrice && x.productName.Contains(searchItem) && x.AlcoholProduct.origin.Contains(country) && x.AlcoholProduct.categoryID == catNum
                                select x).ToListAsync();
            }
            else
            {
                result = await (from x in db.Product
                                where x.price >= minPrice && x.price <= maxPrice && x.productName.Contains(searchItem) && x.AlcoholProduct.origin.Contains(country)
                                select x).ToListAsync();
            }
            


            return result;
        }

        public async Task<IList<Product>> searchCigarProducts(string searchItem, int minPrice, int maxPrice,string cigarBrand, string cigarWrapper, string cigarStrength, string country)
        {
            IList<Product> result = null;
            int cigarStr=0;
            string brand = "";

            if(!cigarStrength.Equals(""))
                cigarStr = int.Parse(cigarStrength);
            if (!cigarBrand.Equals(""))
                brand = (await db.CigarBrands.FindAsync(int.Parse(cigarBrand))).brandName;

            if(!cigarWrapper.Equals("") && !cigarStrength.Equals(""))
            {
                result = await (from x in db.Product
                                where x.price >= minPrice && x.price <= maxPrice && x.productName.Contains(searchItem) && x.CigaresProduct.origin.Contains(country) && x.CigaresProduct.CigarBrand1.brandName.Contains(brand) && x.CigaresProduct.strengthID == cigarStr && x.CigaresProduct.wrapperType.Equals(cigarWrapper)
                                select x).ToListAsync();
            }

            else if(cigarWrapper.Equals("") && !cigarStrength.Equals(""))
            {
                result = await (from x in db.Product
                                where x.price >= minPrice && x.price <= maxPrice && x.productName.Contains(searchItem) && x.CigaresProduct.origin.Contains(country) && x.CigaresProduct.CigarBrand1.brandName.Contains(brand) && x.CigaresProduct.strengthID == cigarStr
                                select x).ToListAsync();
            }

            else if (cigarWrapper.Equals("") && cigarStrength.Equals(""))
            {
                result = await (from x in db.Product
                                where x.price >= minPrice && x.price <= maxPrice && x.productName.Contains(searchItem) && x.CigaresProduct.origin.Contains(country) && x.CigaresProduct.CigarBrand1.brandName.Contains(brand)
                                select x).ToListAsync();
            }

            else if (!cigarWrapper.Equals("") && cigarStrength.Equals(""))
            {
                result = await (from x in db.Product
                                where x.price >= minPrice && x.price <= maxPrice && x.productName.Contains(searchItem) && x.CigaresProduct.origin.Contains(country) && x.CigaresProduct.CigarBrand1.brandName.Contains(brand) && x.CigaresProduct.wrapperType.Equals(cigarWrapper)
                                select x).ToListAsync();
            }

            return result;
        }

        public async Task<IList<Bundle>> searchBundles(IList<Product> ap, IList<Product> cp)
        {
            IList<BundleInfo> allBundleInfos = await db.BundleInfos.ToListAsync();
            IList<BundleInfo> newBundleInfos = new List<BundleInfo>();

            IList<Bundle> allBundles = await db.Bundles.ToListAsync();
            IList<Bundle> newBundles = new List<Bundle>();

            foreach (BundleInfo bi in allBundleInfos)
            {
                if(productExist(ap,bi.productID) || productExist(cp,bi.productID))
                    newBundleInfos.Add(bi);
            }

            foreach(Bundle b in allBundles)
            {
                if (bundleExist(newBundleInfos, b.bundleId))
                    newBundles.Add(b);
            }
            
            return newBundles;
        }

        public Boolean productExist(IList<Product> list, int id)
        {
            foreach(Product p in list)
            {
                if (p.productID == id)
                    return true;
            }
            return false;
        }

        public Boolean bundleExist(IList<BundleInfo> list, int id)
        {
            foreach (BundleInfo p in list)
            {
                if (p.bundleId == id)
                    return true;
            }


            return false;
        }
    }
}