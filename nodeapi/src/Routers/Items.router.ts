import * as express from "express";
import { ItemService } from "../Services/Item.services";
import { app } from "../instances/sequelize";

var ItemServiceModel = new ItemService();
const ItemRouter = express.Router();

ItemRouter.get("/:id", function(req, res, next) {
  ItemServiceModel.GetAllItems(req, res, next);
});

ItemRouter.get("/", function(req, res, next) {
  ItemServiceModel.GetAllItems(req, res, next);
});

ItemRouter.post("/", function(req, res, next) {
  res.json("Post called");
});

ItemRouter.put("/:id", function(req, res, next) {
  res.json("Put/update called");
});

ItemRouter.delete("/:id", function(req, res, next) {
  res.json("Delete called");
});

ItemRouter.get("/GetItemsByStoredPrcedure/Get", function(req, res, next) {
  ItemServiceModel.GetItemsByStoredPrcedure(req, res, next);
});

export default ItemRouter;
