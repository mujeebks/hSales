/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsVoucherInstance, trsVoucherAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsVoucherInstance, trsVoucherAttribute>('trsVoucher', {
    InvID: {
      type: DataTypes.BIGINT,
      allowNull: true,
      defaultValue: '((0))'
    },
    invno: {
      type: DataTypes.BIGINT,
      allowNull: false
    },
    invdate: {
      type: DataTypes.DATE,
      allowNull: true
    },
    vchtype: {
      type: DataTypes.STRING,
      allowNull: true
    },
    drLID: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    crLID: {
      type: DataTypes.INTEGER,
      allowNull: true,
      defaultValue: '((0))'
    },
    amount: {
      type: "MONEY",
      allowNull: true
    },
    refNo: {
      type: DataTypes.STRING,
      allowNull: true
    },
    remarks: {
      type: DataTypes.STRING,
      allowNull: true
    },
    issueingBank: {
      type: DataTypes.STRING,
      allowNull: true
    },
    chequeNo: {
      type: DataTypes.STRING,
      allowNull: true
    },
    spID: {
      type: DataTypes.INTEGER,
      allowNull: true,
      defaultValue: '((0))'
    },
    cancelled: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    crAmount: {
      type: DataTypes.FLOAT,
      allowNull: true
    },
    narration: {
      type: DataTypes.STRING,
      allowNull: true
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    },
    IsReconcil: {
      type: DataTypes.BOOLEAN,
      allowNull: true
    },
    CheqStatus: {
      type: DataTypes.STRING,
      allowNull: true
    },
    ChequeDate: {
      type: DataTypes.DATE,
      allowNull: true
    },
    ReconcilDate: {
      type: DataTypes.DATE,
      allowNull: true
    },
    compName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    userid: {
      type: DataTypes.BIGINT,
      allowNull: true
    }
  }, {
    tableName: 'trsVoucher'
  });
};
