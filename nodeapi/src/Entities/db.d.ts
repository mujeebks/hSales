// tslint:disable
import * as Sequelize from 'sequelize';


// table: companySettings
export interface companySettingsAttribute {
  id:number;
  KeyName?:string;
  ValueName?:string;
}
export interface companySettingsInstance extends Sequelize.Instance<companySettingsAttribute>, companySettingsAttribute { }
export interface companySettingsModel extends Sequelize.Model<companySettingsInstance, companySettingsAttribute> { }

// table: mtrCategory
export interface mtrCategoryAttribute {
  categoryid:number;
  category:string;
  deActive?:boolean;
}
export interface mtrCategoryInstance extends Sequelize.Instance<mtrCategoryAttribute>, mtrCategoryAttribute { }
export interface mtrCategoryModel extends Sequelize.Model<mtrCategoryInstance, mtrCategoryAttribute> { }

// table: InvoiceSettings
export interface InvoiceSettingsAttribute {
  InvoiceId:number;
  InvoiceType?:string;
  FreeQty?:boolean;
  ShowPreview?:boolean;
  PrintonSave?:boolean;
  showConfirm?:boolean;
  columnWidth?:string;
  defPrintScheme?:number;
  editSaleRate?:boolean;
  blnExtraCol?:boolean;
  extraColName?:string;
  extraColMode?:number;
  SSMA_TimeStamp:Date;
  AmtDec?:number;
}
export interface InvoiceSettingsInstance extends Sequelize.Instance<InvoiceSettingsAttribute>, InvoiceSettingsAttribute { }
export interface InvoiceSettingsModel extends Sequelize.Model<InvoiceSettingsInstance, InvoiceSettingsAttribute> { }

// table: mtrArea
export interface mtrAreaAttribute {
  areaID:number;
  areaName?:string;
  deactive?:boolean;
  SSMA_TimeStamp:Date;
}
export interface mtrAreaInstance extends Sequelize.Instance<mtrAreaAttribute>, mtrAreaAttribute { }
export interface mtrAreaModel extends Sequelize.Model<mtrAreaInstance, mtrAreaAttribute> { }

// table: mtrAccountGroup
export interface mtrAccountGroupAttribute {
  AccountGroupid:number;
  GroupName:string;
  BaseGroup?:string;
  deActive?:boolean;
  SSMA_TimeStamp:Date;
}
export interface mtrAccountGroupInstance extends Sequelize.Instance<mtrAccountGroupAttribute>, mtrAccountGroupAttribute { }
export interface mtrAccountGroupModel extends Sequelize.Model<mtrAccountGroupInstance, mtrAccountGroupAttribute> { }

// table: Company
export interface CompanyAttribute {
  companyid:number;
  cdbName?:string;
  companyName?:string;
  blnHide?:boolean;
  productKey?:string;
  licenseKey?:string;
  registered?:boolean;
  expired?:boolean;
  SSMA_TimeStamp:Date;
  AppID?:number;
}
export interface CompanyInstance extends Sequelize.Instance<CompanyAttribute>, CompanyAttribute { }
export interface CompanyModel extends Sequelize.Model<CompanyInstance, CompanyAttribute> { }

// table: mtrCurrency
export interface mtrCurrencyAttribute {
  id:number;
  curName?:string;
  majorSymbol?:string;
  minorSymbol?:string;
  exchangeRate?:number;
  inwordsMode?:number;
  Isdefault?:boolean;
}
export interface mtrCurrencyInstance extends Sequelize.Instance<mtrCurrencyAttribute>, mtrCurrencyAttribute { }
export interface mtrCurrencyModel extends Sequelize.Model<mtrCurrencyInstance, mtrCurrencyAttribute> { }

// table: mtrManufacturer
export interface mtrManufacturerAttribute {
  mnfID:number;
  mnfName?:string;
  deActive?:boolean;
  SSMA_TimeStamp:Date;
}
export interface mtrManufacturerInstance extends Sequelize.Instance<mtrManufacturerAttribute>, mtrManufacturerAttribute { }
export interface mtrManufacturerModel extends Sequelize.Model<mtrManufacturerInstance, mtrManufacturerAttribute> { }

// table: mtrItemImage
export interface mtrItemImageAttribute {
  itemId:number;
  itemImage?:any;
}
export interface mtrItemImageInstance extends Sequelize.Instance<mtrItemImageAttribute>, mtrItemImageAttribute { }
export interface mtrItemImageModel extends Sequelize.Model<mtrItemImageInstance, mtrItemImageAttribute> { }

// table: mtrEmployee
export interface mtrEmployeeAttribute {
  employeeID:number;
  employeeCode?:string;
  employeeName?:string;
  lid?:number;
  address?:string;
  contactNo?:string;
  gender?:string;
  bloodGroup?:string;
  startTime?:string;
  endTime?:string;
  salary?:number;
  deactive?:boolean;
  categoryID?:number;
  SSMA_TimeStamp:Date;
  commPer?:number;
}
export interface mtrEmployeeInstance extends Sequelize.Instance<mtrEmployeeAttribute>, mtrEmployeeAttribute { }
export interface mtrEmployeeModel extends Sequelize.Model<mtrEmployeeInstance, mtrEmployeeAttribute> { }

// table: mtrloyalty
export interface mtrloyaltyAttribute {
  loyaltyID:number;
  loyaltyName?:string;
  saleAmount?:number;
  loyaltyPoint?:number;
  targetPoint?:number;
  deActive?:boolean;
  SSMA_TimeStamp:Date;
}
export interface mtrloyaltyInstance extends Sequelize.Instance<mtrloyaltyAttribute>, mtrloyaltyAttribute { }
export interface mtrloyaltyModel extends Sequelize.Model<mtrloyaltyInstance, mtrloyaltyAttribute> { }

// table: mtrledger
export interface mtrledgerAttribute {
  lid:number;
  ledger:string;
  LedgerName?:string;
  accid?:number;
  contactno?:string;
  phoneNo?:string;
  address?:string;
  tinno?:string;
  areaid?:number;
  deActive?:boolean;
  plID?:number;
  email?:string;
  loyaltyID?:number;
  regionalName?:string;
  remarks?:string;
  DiscPer?:number;
  SSMA_TimeStamp:Date;
  creditlimit?:number;
}
export interface mtrledgerInstance extends Sequelize.Instance<mtrledgerAttribute>, mtrledgerAttribute { }
export interface mtrledgerModel extends Sequelize.Model<mtrledgerInstance, mtrledgerAttribute> { }

// table: mtrPricelist
export interface mtrPricelistAttribute {
  plId:number;
  plName?:string;
  deActive?:boolean;
  SSMA_TimeStamp:Date;
}
export interface mtrPricelistInstance extends Sequelize.Instance<mtrPricelistAttribute>, mtrPricelistAttribute { }
export interface mtrPricelistModel extends Sequelize.Model<mtrPricelistInstance, mtrPricelistAttribute> { }

// table: mtrProduct
export interface mtrProductAttribute {
  prodID:number;
  prodCode?:string;
  prodName:string;
  categoryid:number;
  sTax?:any;
  pTax?:any;
  staxincl?:boolean;
  ptaxincl?:boolean;
  mnf?:string;
  mainUnit?:string;
  Rack?:string;
  prate?:any;
  srate?:any;
  mrp?:any;
  active?:boolean;
  igstPer?:number;
  rol?:number;
  remarks?:string;
  prateper?:number;
  srateper?:number;
  regionalName?:string;
  blnExpiry?:boolean;
  SSMA_TimeStamp:Date;
  alternateUnit?:string;
  convfactor?:number;
  itemClassID?:number;
  Batch?:string;
  hsnCode?:string;
  IsBoardItem?:boolean;
  pluCode?:string;
  cessPer?:number;
  AddCess?:number;
  OSRate?:number;
  outerBatch?:string;
  SPComPer?:number;
}
export interface mtrProductInstance extends Sequelize.Instance<mtrProductAttribute>, mtrProductAttribute { }
export interface mtrProductModel extends Sequelize.Model<mtrProductInstance, mtrProductAttribute> { }

// table: PrintSettings
export interface PrintSettingsAttribute {
  Id:number;
  InvoiceType?:string;
  SchemeName?:string;
  DosMode?:boolean;
  paperWidth?:number;
  Leftmargin?:number;
  paperFeed?:number;
  reverseFeed?:number;
  footerLines?:number;
  fontIndex?:number;
  fontSizeIndex?:number;
  Nooflines?:number;
  blnVarlines?:boolean;
  blnNormal?:boolean;
  printer?:string;
  blnprintValues?:string;
  gridColHeadWidth?:string;
  printtextvalues?:string;
  SSMA_TimeStamp:Date;
}
export interface PrintSettingsInstance extends Sequelize.Instance<PrintSettingsAttribute>, PrintSettingsAttribute { }
export interface PrintSettingsModel extends Sequelize.Model<PrintSettingsInstance, PrintSettingsAttribute> { }

// table: mtrRole
export interface mtrRoleAttribute {
  roleId:number;
  roleName?:string;
  deActive?:boolean;
  SSMA_TimeStamp:Date;
}
export interface mtrRoleInstance extends Sequelize.Instance<mtrRoleAttribute>, mtrRoleAttribute { }
export interface mtrRoleModel extends Sequelize.Model<mtrRoleInstance, mtrRoleAttribute> { }

// table: mtrRolePermission
export interface mtrRolePermissionAttribute {
  roleId:number;
  screenName?:string;
  isread?:boolean;
  isadd?:boolean;
  isedit?:boolean;
  iscancel?:boolean;
  isdelete?:boolean;
  isprint?:boolean;
  isexport?:boolean;
  isemail?:boolean;
  issms?:boolean;
  SSMA_TimeStamp:Date;
}
export interface mtrRolePermissionInstance extends Sequelize.Instance<mtrRolePermissionAttribute>, mtrRolePermissionAttribute { }
export interface mtrRolePermissionModel extends Sequelize.Model<mtrRolePermissionInstance, mtrRolePermissionAttribute> { }

// table: mtrUser
export interface mtrUserAttribute {
  userId:number;
  userName?:string;
  userPass?:string;
  roleId?:number;
  deActive?:boolean;
  SSMA_TimeStamp:Date;
}
export interface mtrUserInstance extends Sequelize.Instance<mtrUserAttribute>, mtrUserAttribute { }
export interface mtrUserModel extends Sequelize.Model<mtrUserInstance, mtrUserAttribute> { }

// table: trsAttandance
export interface trsAttandanceAttribute {
  AttID?:number;
  employeeID?:number;
  InvDate?:Date;
  status?:string;
  Quantity?:number;
  Amount?:number;
  TotAmount?:number;
  Advance?:number;
  Incentive?:number;
  SSMA_TimeStamp:Date;
}
export interface trsAttandanceInstance extends Sequelize.Instance<trsAttandanceAttribute>, trsAttandanceAttribute { }
export interface trsAttandanceModel extends Sequelize.Model<trsAttandanceInstance, trsAttandanceAttribute> { }

// table: trsAccounts
export interface trsAccountsAttribute {
  Invid?:number;
  InvType?:string;
  InvDate?:Date;
  Drlid?:number;
  Crlid?:number;
  AmountD?:any;
  AmountC?:any;
  invNo?:string;
  remarks?:string;
  IsHold?:boolean;
}
export interface trsAccountsInstance extends Sequelize.Instance<trsAccountsAttribute>, trsAccountsAttribute { }
export interface trsAccountsModel extends Sequelize.Model<trsAccountsInstance, trsAccountsAttribute> { }

// table: trsloyalty
export interface trsloyaltyAttribute {
  SaleID:number;
  loyaltyID:number;
  LID:number;
  saleAmount?:number;
  PointIn?:number;
  PointOut?:number;
  SSMA_TimeStamp:Date;
}
export interface trsloyaltyInstance extends Sequelize.Instance<trsloyaltyAttribute>, trsloyaltyAttribute { }
export interface trsloyaltyModel extends Sequelize.Model<trsloyaltyInstance, trsloyaltyAttribute> { }

// table: trsBOM
export interface trsBOMAttribute {
  bomId:number;
  bomName?:string;
  description?:string;
  deActive?:boolean;
  SSMA_TimeStamp:Date;
}
export interface trsBOMInstance extends Sequelize.Instance<trsBOMAttribute>, trsBOMAttribute { }
export interface trsBOMModel extends Sequelize.Model<trsBOMInstance, trsBOMAttribute> { }

// table: trsBOMDetail
export interface trsBOMDetailAttribute {
  bomId:number;
  prodID:number;
  qty?:number;
  rOrder?:number;
  bomMode?:number;
  blnAltUnit?:boolean;
  SSMA_TimeStamp:Date;
}
export interface trsBOMDetailInstance extends Sequelize.Instance<trsBOMDetailAttribute>, trsBOMDetailAttribute { }
export interface trsBOMDetailModel extends Sequelize.Model<trsBOMDetailInstance, trsBOMDetailAttribute> { }

// table: trsItemOffer
export interface trsItemOfferAttribute {
  id:number;
  offerName?:string;
  startDate?:Date;
  startTime?:Date;
  endDate?:Date;
  endTime?:Date;
  Itemid:number;
  batch?:string;
  mrp?:number;
  rate?:number;
  Discount?:number;
  netRate?:number;
  deActive?:boolean;
}
export interface trsItemOfferInstance extends Sequelize.Instance<trsItemOfferAttribute>, trsItemOfferAttribute { }
export interface trsItemOfferModel extends Sequelize.Model<trsItemOfferInstance, trsItemOfferAttribute> { }

// table: trsOrderDetail
export interface trsOrderDetailAttribute {
  invID:number;
  itemid?:number;
  taxper?:number;
  rate?:number;
  mrp?:any;
  quantity?:number;
  freeQty?:number;
  grossAmount?:number;
  Discount?:number;
  taxable?:number;
  taxAmount?:number;
  netAmount?:number;
  batch?:string;
  cost?:number;
  saleValue?:number;
  profit?:number;
  rOrder?:number;
  extraColumn?:string;
  SSMA_TimeStamp:Date;
  unitfactor?:number;
  blnAltUnit?:boolean;
  unit?:string;
  expDate?:string;
  sRate?:number;
}
export interface trsOrderDetailInstance extends Sequelize.Instance<trsOrderDetailAttribute>, trsOrderDetailAttribute { }
export interface trsOrderDetailModel extends Sequelize.Model<trsOrderDetailInstance, trsOrderDetailAttribute> { }

// table: trsOrder
export interface trsOrderAttribute {
  invid:number;
  invType?:string;
  invno?:string;
  invdate?:Date;
  party?:string;
  lid?:number;
  mop?:string;
  taxType?:string;
  contactno?:string;
  remarks?:string;
  address?:string;
  SalePersonID?:number;
  grossAmount?:number;
  Discount?:number;
  taxableAmount?:number;
  taxAmount?:number;
  Exempted?:any;
  roundOff?:number;
  roundOffMode?:number;
  cashDiscount?:number;
  billAmount?:number;
  otherExpense?:number;
  freightCoolie?:number;
  cost?:number;
  SaleValues?:number;
  BillProfit?:number;
  blnProcessed?:boolean;
  cancelled?:boolean;
  tinNo?:string;
  SSMA_TimeStamp:Date;
  AutoNo?:number;
  referenceNo?:string;
  costfactor?:number;
  compName?:string;
  userid?:number;
  OrderDetail?:string;
  DespatchDetail?:string;
  cessAmount?:number;
  AddCessAmt?:number;
  rndCutoff?:number;
}
export interface trsOrderInstance extends Sequelize.Instance<trsOrderAttribute>, trsOrderAttribute { }
export interface trsOrderModel extends Sequelize.Model<trsOrderInstance, trsOrderAttribute> { }

// table: trsPricelist
export interface trsPricelistAttribute {
  plId:number;
  plName?:string;
  prodId?:number;
  price?:number;
  SSMA_TimeStamp:Date;
}
export interface trsPricelistInstance extends Sequelize.Instance<trsPricelistAttribute>, trsPricelistAttribute { }
export interface trsPricelistModel extends Sequelize.Model<trsPricelistInstance, trsPricelistAttribute> { }

// table: trsSalaryPay
export interface trsSalaryPayAttribute {
  payID?:number;
  employeeID?:number;
  InvDate?:Date;
  fromDate?:Date;
  toDate?:Date;
  daysWorked?:string;
  Quantity?:number;
  Amount?:number;
  Advance?:number;
  Incentive?:number;
  BalAmount?:number;
  SSMA_TimeStamp:Date;
}
export interface trsSalaryPayInstance extends Sequelize.Instance<trsSalaryPayAttribute>, trsSalaryPayAttribute { }
export interface trsSalaryPayModel extends Sequelize.Model<trsSalaryPayInstance, trsSalaryPayAttribute> { }

// table: trsPurchasedetail
export interface trsPurchasedetailAttribute {
  invid?:number;
  itemid?:number;
  taxper?:number;
  rate?:any;
  quantity?:number;
  freeQty?:number;
  grossAmount?:any;
  discount?:any;
  taxable?:any;
  taxAmount?:any;
  netAmount?:any;
  Batch?:string;
  sRate?:number;
  mrp?:number;
  cost?:number;
  rOrder?:number;
  SSMA_TimeStamp:Date;
  unitfactor?:number;
  blnAltUnit?:boolean;
  expDate?:string;
  unit?:string;
  extraColumn?:string;
  profit?:number;
  saleValue?:number;
}
export interface trsPurchasedetailInstance extends Sequelize.Instance<trsPurchasedetailAttribute>, trsPurchasedetailAttribute { }
export interface trsPurchasedetailModel extends Sequelize.Model<trsPurchasedetailInstance, trsPurchasedetailAttribute> { }

// table: trsPurchase
export interface trsPurchaseAttribute {
  invid:number;
  invType?:string;
  invno:string;
  invdate?:Date;
  party?:string;
  lid?:number;
  mop?:string;
  taxType?:string;
  contactno?:string;
  remarks?:string;
  address?:string;
  SalePersonID?:number;
  grossAmount?:any;
  Discount?:any;
  taxableAmount?:any;
  taxAmount?:any;
  Exempted?:any;
  roundOff?:any;
  cashDiscount?:any;
  billAmount?:any;
  roundOffMode?:number;
  otherExpense?:number;
  freightCoolie?:number;
  referenceNo?:string;
  cancelled?:boolean;
  blnOrder?:boolean;
  OrderID?:number;
  tinNo?:string;
  SSMA_TimeStamp:Date;
  costfactor?:number;
  compName?:string;
  userid?:number;
  OrderDetail?:string;
  DespatchDetail?:string;
  cessAmount?:number;
  AddCessAmt?:number;
  rndCutoff?:number;
}
export interface trsPurchaseInstance extends Sequelize.Instance<trsPurchaseAttribute>, trsPurchaseAttribute { }
export interface trsPurchaseModel extends Sequelize.Model<trsPurchaseInstance, trsPurchaseAttribute> { }

// table: trsStock
export interface trsStockAttribute {
  Invid?:number;
  InvType?:string;
  Invdate?:Date;
  itemid?:number;
  qtyin?:number;
  qtyout?:number;
  Batch?:string;
  SSMA_TimeStamp:Date;
  mrp?:number;
  sRate?:number;
  pRate?:number;
  lid?:number;
  invNo?:string;
}
export interface trsStockInstance extends Sequelize.Instance<trsStockAttribute>, trsStockAttribute { }
export interface trsStockModel extends Sequelize.Model<trsStockInstance, trsStockAttribute> { }

// table: trsSale
export interface trsSaleAttribute {
  invid:number;
  invType?:string;
  invno:string;
  invdate?:Date;
  party?:string;
  lid?:number;
  mop?:string;
  taxType?:string;
  contactno?:string;
  remarks?:string;
  address?:string;
  SalePersonID?:number;
  grossAmount?:any;
  Discount?:any;
  taxableAmount?:any;
  taxAmount?:any;
  Exempted?:any;
  roundOff?:any;
  cashDiscount?:any;
  billAmount?:any;
  roundOffMode?:number;
  otherExpense?:number;
  freightCoolie?:number;
  cost?:number;
  saleValues?:number;
  BillProfit?:number;
  blnOrder?:boolean;
  OrderID?:number;
  cancelled?:boolean;
  loyID?:number;
  pointIn?:number;
  pointOut?:number;
  cardNo?:string;
  cardExpMnth?:string;
  cardExpyr?:string;
  cardcvNo?:string;
  tinNo?:string;
  cardName?:string;
  SSMA_TimeStamp:Date;
  AutoNo?:number;
  compName?:string;
  userid?:number;
  OrderDetail?:string;
  DespatchDetail?:string;
  cessAmount?:number;
  AddCessAmt?:number;
  rndCutoff?:number;
  creditAmount?:number;
  cashAmount?:number;
  cardAmount?:number;
  itemDiscount?:number;
}
export interface trsSaleInstance extends Sequelize.Instance<trsSaleAttribute>, trsSaleAttribute { }
export interface trsSaleModel extends Sequelize.Model<trsSaleInstance, trsSaleAttribute> { }

// table: trsSaledetail
export interface trsSaledetailAttribute {
  invid?:number;
  itemid?:number;
  taxper?:number;
  rate?:any;
  mrp?:any;
  quantity?:number;
  freeQty?:number;
  grossAmount?:any;
  discount?:any;
  taxable?:any;
  taxAmount?:any;
  netAmount?:any;
  Batch?:string;
  cost?:number;
  saleValue?:number;
  profit?:number;
  rOrder?:number;
  extraColumn?:string;
  SSMA_TimeStamp:Date;
  unitfactor?:number;
  blnAltUnit?:boolean;
  unit?:string;
  expDate?:string;
}
export interface trsSaledetailInstance extends Sequelize.Instance<trsSaledetailAttribute>, trsSaledetailAttribute { }
export interface trsSaledetailModel extends Sequelize.Model<trsSaledetailInstance, trsSaledetailAttribute> { }

// table: trsStockQty
export interface trsStockQtyAttribute {
  ItemID?:number;
  Batch?:string;
  Prate?:number;
  SRate?:number;
  Mrp?:number;
  cost?:number;
  intBatch?:number;
  SSMA_TimeStamp:Date;
  deActive?:boolean;
  expDate?:string;
}
export interface trsStockQtyInstance extends Sequelize.Instance<trsStockQtyAttribute>, trsStockQtyAttribute { }
export interface trsStockQtyModel extends Sequelize.Model<trsStockQtyInstance, trsStockQtyAttribute> { }

// table: trsStockJournaldetail
export interface trsStockJournaldetailAttribute {
  invid?:number;
  itemid?:number;
  taxper?:number;
  rate?:any;
  quantity?:number;
  freeQty?:number;
  grossAmount?:any;
  discount?:any;
  taxable?:any;
  taxAmount?:any;
  netAmount?:any;
  Batch?:string;
  sRate?:number;
  mrp?:number;
  cost?:number;
  saleValue?:number;
  profit?:number;
  productionid?:string;
  rOrder?:number;
  SSMA_TimeStamp:Date;
  unitfactor?:number;
  blnAltUnit?:boolean;
  expDate?:string;
  unit?:string;
  igstPer?:number;
  extraColumn?:string;
}
export interface trsStockJournaldetailInstance extends Sequelize.Instance<trsStockJournaldetailAttribute>, trsStockJournaldetailAttribute { }
export interface trsStockJournaldetailModel extends Sequelize.Model<trsStockJournaldetailInstance, trsStockJournaldetailAttribute> { }

// table: trsStockJournal
export interface trsStockJournalAttribute {
  invid:number;
  invType?:string;
  invno:string;
  invdate?:Date;
  party?:string;
  lid?:number;
  mop?:string;
  taxType?:string;
  contactno?:string;
  remarks?:string;
  address?:string;
  SalePersonID?:number;
  grossAmount?:any;
  Discount?:any;
  taxableAmount?:any;
  taxAmount?:any;
  Exempted?:any;
  roundOff?:any;
  cashDiscount?:any;
  billAmount?:any;
  otherExpense?:number;
  freightCoolie?:number;
  roundOffMode?:number;
  cost?:number;
  SaleValues?:number;
  BillProfit?:number;
  labourCost?:number;
  otherCost?:number;
  cancelled?:boolean;
  referenceNo?:string;
  tinNo?:string;
  SSMA_TimeStamp:Date;
  costfactor?:number;
  compName?:string;
  userid?:number;
  OrderDetail?:string;
  DespatchDetail?:string;
  cessAmount?:number;
  AddCessAmt?:number;
  rndCutoff?:number;
}
export interface trsStockJournalInstance extends Sequelize.Instance<trsStockJournalAttribute>, trsStockJournalAttribute { }
export interface trsStockJournalModel extends Sequelize.Model<trsStockJournalInstance, trsStockJournalAttribute> { }

// table: trsVoidItem
export interface trsVoidItemAttribute {
  userId:number;
  userName?:string;
  screenName?:string;
  billNo?:string;
  billDate?:Date;
  item?:string;
  batch?:string;
  rate?:string;
  mrp?:string;
  logDate?:Date;
  logTime?:Date;
}
export interface trsVoidItemInstance extends Sequelize.Instance<trsVoidItemAttribute>, trsVoidItemAttribute { }
export interface trsVoidItemModel extends Sequelize.Model<trsVoidItemInstance, trsVoidItemAttribute> { }

// table: trsVoucher
export interface trsVoucherAttribute {
  InvID?:number;
  invno:number;
  invdate?:Date;
  vchtype?:string;
  drLID?:number;
  crLID?:number;
  amount?:any;
  refNo?:string;
  remarks?:string;
  issueingBank?:string;
  chequeNo?:string;
  spID?:number;
  cancelled?:boolean;
  crAmount?:number;
  narration?:string;
  SSMA_TimeStamp:Date;
  IsReconcil?:boolean;
  CheqStatus?:string;
  ChequeDate?:Date;
  ReconcilDate?:Date;
  compName?:string;
  userid?:number;
}
export interface trsVoucherInstance extends Sequelize.Instance<trsVoucherAttribute>, trsVoucherAttribute { }
export interface trsVoucherModel extends Sequelize.Model<trsVoucherInstance, trsVoucherAttribute> { }

// table: trsUserlog
export interface trsUserlogAttribute {
  userId:number;
  userName?:string;
  screenName?:string;
  actionType?:string;
  logDate?:Date;
  logTime?:Date;
  remarks?:string;
}
export interface trsUserlogInstance extends Sequelize.Instance<trsUserlogAttribute>, trsUserlogAttribute { }
export interface trsUserlogModel extends Sequelize.Model<trsUserlogInstance, trsUserlogAttribute> { }
