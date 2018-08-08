/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {companySettingsInstance, companySettingsAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<companySettingsInstance, companySettingsAttribute>('companySettings', {
    id: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true
    },
    KeyName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    ValueName: {
      type: DataTypes.STRING,
      allowNull: true
    }
  }, {
    tableName: 'companySettings'
  });
};
