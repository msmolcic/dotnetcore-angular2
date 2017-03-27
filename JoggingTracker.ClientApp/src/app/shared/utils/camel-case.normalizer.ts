import * as _ from 'lodash';

export function normalizeCamelCase (key: string): string {
    let properties = key.split('.');

    for (var prop in properties) 
      properties[prop] = _.camelCase(properties[prop]);

    return properties.join('.');
}