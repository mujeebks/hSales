/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsVoidItemInstance, trsVoidItemAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsVoidItemInstance, trsVoidItemAttribute>('trsVoidItem', {
    userId: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    userName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    screenName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    billNo: {
      type: DataTypes.STRING,
      allowNull: true
    },
    billDate: {
      type: DataTypes.DATE,
      allowNull: true
    },
    item: {
      type: DataTypes.STRING,
      allowNull: true
    },
    batch: {
      type: DataTypes.STRING,
      allowNull: true
    },
    rate: {
      type: DataTypes.STRING,
      allowNull: true
    },
    mrp: {
      type: DataTypes.STRING,
      allowNull: true
    },
    logDate: {
      type: DataTypes.DATE,
      allowNull: true
    },
    logTime: {
      type: DataTypes.DATE,
      allowNull: true
    }
  }, {
    tableName: 'trsVoidItem'
  });
};
