/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {mtrManufacturerInstance, mtrManufacturerAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<mtrManufacturerInstance, mtrManufacturerAttribute>('mtrManufacturer', {
    mnfID: {
      type: DataTypes.BIGINT,
      allowNull: false,
      defaultValue: '((0))',
      primaryKey: true
    },
    mnfName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    deActive: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    }
  }, {
    tableName: 'mtrManufacturer'
  });
};
