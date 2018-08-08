import { app } from "./instances/sequelize";

import { FindCaller } from "./Helper/Helper.services";
import CategoriesRouter from "./Routers/Category.router";

// import ItemRouter from "./Routers/Items.router";
// import DepartmentRouter from "./Routers/Department.router";
// import ErrorRouter from "./Routers/Error.router";

const bodyParser = require("body-parser");


app.use(bodyParser.urlencoded({
    extended: true
}));

/**bodyParser.json(options)
 * Parses the text as JSON and exposes the resulting object on req.body.
 */
app.use(bodyParser.json());
app.use(bodyParser());


app.use('/api/Masters/Categories',FindCaller,CategoriesRouter);
// app.use('/api/data/Items',FindCaller,ItemRouter);
// app.use('/api/data/Departments',FindCaller,DepartmentRouter);
// app.use('/api/data/Errors',FindCaller,ErrorRouter);



// app.use(function(req, res, next) {
//     res.header("Access-Control-Allow-Origin", "*");
//     res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
//     next();
//   });



// app.use(function(req, res, next) {
//     res.header("Access-Control-Allow-Origin", "*");
//     res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
//     next();
//   });
  

