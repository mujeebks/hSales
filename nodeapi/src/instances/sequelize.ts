import * as Sequelize from "sequelize";
var cors = require('cors')
import * as express from "express";
var ip = require('ip');

//for remote db
// var sequelize = new Sequelize("Sequilize", "sa", "abc123*", {
//   dialect: "mssql",
//   host: "192.168.10.106",
//   username: "sa",
//   password: "abc123*",
//   dialectOptions: {
//     port: 8016
//   },
//   define: {
//     timestamps: false  // I don't want timestamp fields by default
//   },
// });


//for local db

var sequelize = new Sequelize("XeniaPOSVLite", "sa", "abc123*", {
  dialect: "mssql",
  host: "localhost",
  username: "sa",
  password: "abc123*",
  dialectOptions: {
    instanceName: "SQL2016",
    
},
  define: {
    timestamps: false  // I don't want timestamp fields by default
  },
});


sequelize
  .authenticate()
  .then(() => {
    console.log("Connection has been established successfully.API running....");
  })
  .catch(err => {
    console.error("Unable to connect to the database:", err);
  });
var app = express();
app.use(cors())
const port = 4001;
app.listen(port, ip.address(), () => {
  console.log(`server is listening on port ${port}`);
  console.log(`server is listening on ip  ${ip.address()}`);
  
});

export { sequelize, app };
