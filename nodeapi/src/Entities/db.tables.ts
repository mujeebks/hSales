// tslint:disable
import * as path from 'path';
import * as sequelize from 'sequelize';
import * as def from './db';

export interface ITables {
  InvoiceSettings:def.InvoiceSettingsModel;
  companySettings:def.companySettingsModel;
  Company:def.CompanyModel;
  mtrCategory:def.mtrCategoryModel;
  mtrArea:def.mtrAreaModel;
  mtrAccountGroup:def.mtrAccountGroupModel;
  mtrItemImage:def.mtrItemImageModel;
  mtrCurrency:def.mtrCurrencyModel;
  mtrEmployee:def.mtrEmployeeModel;
  mtrledger:def.mtrledgerModel;
  mtrManufacturer:def.mtrManufacturerModel;
  mtrloyalty:def.mtrloyaltyModel;
  mtrRolePermission:def.mtrRolePermissionModel;
  mtrPricelist:def.mtrPricelistModel;
  mtrRole:def.mtrRoleModel;
  mtrProduct:def.mtrProductModel;
  mtrUser:def.mtrUserModel;
  PrintSettings:def.PrintSettingsModel;
  trsItemOffer:def.trsItemOfferModel;
  trsBOM:def.trsBOMModel;
  trsAttandance:def.trsAttandanceModel;
  trsAccounts:def.trsAccountsModel;
  trsBOMDetail:def.trsBOMDetailModel;
  trsloyalty:def.trsloyaltyModel;
  trsPurchasedetail:def.trsPurchasedetailModel;
  trsOrder:def.trsOrderModel;
  trsOrderDetail:def.trsOrderDetailModel;
  trsPricelist:def.trsPricelistModel;
  trsPurchase:def.trsPurchaseModel;
  trsSalaryPay:def.trsSalaryPayModel;
  trsStockJournaldetail:def.trsStockJournaldetailModel;
  trsSaledetail:def.trsSaledetailModel;
  trsSale:def.trsSaleModel;
  trsStock:def.trsStockModel;
  trsStockJournal:def.trsStockJournalModel;
  trsStockQty:def.trsStockQtyModel;
  trsUserlog:def.trsUserlogModel;
  trsVoidItem:def.trsVoidItemModel;
  trsVoucher:def.trsVoucherModel;
}

export const getModels = function(seq:sequelize.Sequelize):ITables {
  const tables:ITables = {
    InvoiceSettings: seq.import(path.join(__dirname, './InvoiceSettings')),
    companySettings: seq.import(path.join(__dirname, './companySettings')),
    Company: seq.import(path.join(__dirname, './Company')),
    mtrCategory: seq.import(path.join(__dirname, './mtrCategory')),
    mtrArea: seq.import(path.join(__dirname, './mtrArea')),
    mtrAccountGroup: seq.import(path.join(__dirname, './mtrAccountGroup')),
    mtrItemImage: seq.import(path.join(__dirname, './mtrItemImage')),
    mtrCurrency: seq.import(path.join(__dirname, './mtrCurrency')),
    mtrEmployee: seq.import(path.join(__dirname, './mtrEmployee')),
    mtrledger: seq.import(path.join(__dirname, './mtrledger')),
    mtrManufacturer: seq.import(path.join(__dirname, './mtrManufacturer')),
    mtrloyalty: seq.import(path.join(__dirname, './mtrloyalty')),
    mtrRolePermission: seq.import(path.join(__dirname, './mtrRolePermission')),
    mtrPricelist: seq.import(path.join(__dirname, './mtrPricelist')),
    mtrRole: seq.import(path.join(__dirname, './mtrRole')),
    mtrProduct: seq.import(path.join(__dirname, './mtrProduct')),
    mtrUser: seq.import(path.join(__dirname, './mtrUser')),
    PrintSettings: seq.import(path.join(__dirname, './PrintSettings')),
    trsItemOffer: seq.import(path.join(__dirname, './trsItemOffer')),
    trsBOM: seq.import(path.join(__dirname, './trsBOM')),
    trsAttandance: seq.import(path.join(__dirname, './trsAttandance')),
    trsAccounts: seq.import(path.join(__dirname, './trsAccounts')),
    trsBOMDetail: seq.import(path.join(__dirname, './trsBOMDetail')),
    trsloyalty: seq.import(path.join(__dirname, './trsloyalty')),
    trsPurchasedetail: seq.import(path.join(__dirname, './trsPurchasedetail')),
    trsOrder: seq.import(path.join(__dirname, './trsOrder')),
    trsOrderDetail: seq.import(path.join(__dirname, './trsOrderDetail')),
    trsPricelist: seq.import(path.join(__dirname, './trsPricelist')),
    trsPurchase: seq.import(path.join(__dirname, './trsPurchase')),
    trsSalaryPay: seq.import(path.join(__dirname, './trsSalaryPay')),
    trsStockJournaldetail: seq.import(path.join(__dirname, './trsStockJournaldetail')),
    trsSaledetail: seq.import(path.join(__dirname, './trsSaledetail')),
    trsSale: seq.import(path.join(__dirname, './trsSale')),
    trsStock: seq.import(path.join(__dirname, './trsStock')),
    trsStockJournal: seq.import(path.join(__dirname, './trsStockJournal')),
    trsStockQty: seq.import(path.join(__dirname, './trsStockQty')),
    trsUserlog: seq.import(path.join(__dirname, './trsUserlog')),
    trsVoidItem: seq.import(path.join(__dirname, './trsVoidItem')),
    trsVoucher: seq.import(path.join(__dirname, './trsVoucher')),
  };
  return tables;
};
