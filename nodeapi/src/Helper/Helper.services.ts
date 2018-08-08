let FindCaller = (req, res, next) => {
  var ip = req.header("x-forwarded-for") || req.connection.remoteAddress;

  // log in different colors
  // https://stackoverflow.com/questions/9781218/how-to-change-node-jss-console-font-color


  console.log('\x1b[33m%s\x1b[0m', "Api Called by Ip:" + ip + " Method :" + req.method); 
  // To Move on
  next();
};

export { FindCaller };
