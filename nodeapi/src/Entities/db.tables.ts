// tslint:disable
import * as path from 'path';
import * as sequelize from 'sequelize';
import * as def from './db';

export interface ITables {
  companySettings:def.companySettingsModel;
  mtrCategory:def.mtrCategoryModel;
  InvoiceSettings:def.InvoiceSettingsModel;
  mtrArea:def.mtrAreaModel;
  mtrAccountGroup:def.mtrAccountGroupModel;
  Company:def.CompanyModel;
  mtrCurrency:def.mtrCurrencyModel;
  mtrManufacturer:def.mtrManufacturerModel;
  mtrItemImage:def.mtrItemImageModel;
  mtrEmployee:def.mtrEmployeeModel;
  mtrloyalty:def.mtrloyaltyModel;
  mtrledger:def.mtrledgerModel;
  mtrPricelist:def.mtrPricelistModel;
  mtrProduct:def.mtrProductModel;
  PrintSettings:def.PrintSettingsModel;
  mtrRole:def.mtrRoleModel;
  mtrRolePermission:def.mtrRolePermissionModel;
  mtrUser:def.mtrUserModel;
  trsAttandance:def.trsAttandanceModel;
  trsAccounts:def.trsAccountsModel;
  trsloyalty:def.trsloyaltyModel;
  trsBOM:def.trsBOMModel;
  trsBOMDetail:def.trsBOMDetailModel;
  trsItemOffer:def.trsItemOfferModel;
  trsOrderDetail:def.trsOrderDetailModel;
  trsOrder:def.trsOrderModel;
  trsPricelist:def.trsPricelistModel;
  trsSalaryPay:def.trsSalaryPayModel;
  trsPurchasedetail:def.trsPurchasedetailModel;
  trsPurchase:def.trsPurchaseModel;
  trsStock:def.trsStockModel;
  trsSale:def.trsSaleModel;
  trsSaledetail:def.trsSaledetailModel;
  trsStockQty:def.trsStockQtyModel;
  trsStockJournaldetail:def.trsStockJournaldetailModel;
  trsStockJournal:def.trsStockJournalModel;
  trsVoidItem:def.trsVoidItemModel;
  trsVoucher:def.trsVoucherModel;
  trsUserlog:def.trsUserlogModel;
}

export const getModels = function(seq:sequelize.Sequelize):ITables {
  const tables:ITables = {
    companySettings: seq.import(path.join(__dirname, './companySettings')),
    mtrCategory: seq.import(path.join(__dirname, './mtrCategory')),
    InvoiceSettings: seq.import(path.join(__dirname, './InvoiceSettings')),
    mtrArea: seq.import(path.join(__dirname, './mtrArea')),
    mtrAccountGroup: seq.import(path.join(__dirname, './mtrAccountGroup')),
    Company: seq.import(path.join(__dirname, './Company')),
    mtrCurrency: seq.import(path.join(__dirname, './mtrCurrency')),
    mtrManufacturer: seq.import(path.join(__dirname, './mtrManufacturer')),
    mtrItemImage: seq.import(path.join(__dirname, './mtrItemImage')),
    mtrEmployee: seq.import(path.join(__dirname, './mtrEmployee')),
    mtrloyalty: seq.import(path.join(__dirname, './mtrloyalty')),
    mtrledger: seq.import(path.join(__dirname, './mtrledger')),
    mtrPricelist: seq.import(path.join(__dirname, './mtrPricelist')),
    mtrProduct: seq.import(path.join(__dirname, './mtrProduct')),
    PrintSettings: seq.import(path.join(__dirname, './PrintSettings')),
    mtrRole: seq.import(path.join(__dirname, './mtrRole')),
    mtrRolePermission: seq.import(path.join(__dirname, './mtrRolePermission')),
    mtrUser: seq.import(path.join(__dirname, './mtrUser')),
    trsAttandance: seq.import(path.join(__dirname, './trsAttandance')),
    trsAccounts: seq.import(path.join(__dirname, './trsAccounts')),
    trsloyalty: seq.import(path.join(__dirname, './trsloyalty')),
    trsBOM: seq.import(path.join(__dirname, './trsBOM')),
    trsBOMDetail: seq.import(path.join(__dirname, './trsBOMDetail')),
    trsItemOffer: seq.import(path.join(__dirname, './trsItemOffer')),
    trsOrderDetail: seq.import(path.join(__dirname, './trsOrderDetail')),
    trsOrder: seq.import(path.join(__dirname, './trsOrder')),
    trsPricelist: seq.import(path.join(__dirname, './trsPricelist')),
    trsSalaryPay: seq.import(path.join(__dirname, './trsSalaryPay')),
    trsPurchasedetail: seq.import(path.join(__dirname, './trsPurchasedetail')),
    trsPurchase: seq.import(path.join(__dirname, './trsPurchase')),
    trsStock: seq.import(path.join(__dirname, './trsStock')),
    trsSale: seq.import(path.join(__dirname, './trsSale')),
    trsSaledetail: seq.import(path.join(__dirname, './trsSaledetail')),
    trsStockQty: seq.import(path.join(__dirname, './trsStockQty')),
    trsStockJournaldetail: seq.import(path.join(__dirname, './trsStockJournaldetail')),
    trsStockJournal: seq.import(path.join(__dirname, './trsStockJournal')),
    trsVoidItem: seq.import(path.join(__dirname, './trsVoidItem')),
    trsVoucher: seq.import(path.join(__dirname, './trsVoucher')),
    trsUserlog: seq.import(path.join(__dirname, './trsUserlog')),
  };
  return tables;
};
