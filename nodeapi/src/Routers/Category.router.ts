import * as express from "express";
import { CategoryService } from "../Services/Category.services";


var CategoryServiceModel = new CategoryService();

const CategoriesRouter = express.Router();

CategoriesRouter.get("/:id", function(req, res, next) {

  CategoryServiceModel.GetAllCategories(req, res, next);
});

CategoriesRouter.get("/", function(req, res, next) {

  return CategoryServiceModel.GetAllCategories(req, res, next);
});

CategoriesRouter.post("/", function(req, res, next) {
  CategoryServiceModel.SaveCategories(req, res, next);
});

CategoriesRouter.put("/:id", function(req, res, next) {
  CategoryServiceModel.SaveCategories(req, res, next);
});

CategoriesRouter.delete("/:id", function(req, res, next) {
  CategoryServiceModel.DeleteCategory(req, res, next);
});

export default CategoriesRouter;
