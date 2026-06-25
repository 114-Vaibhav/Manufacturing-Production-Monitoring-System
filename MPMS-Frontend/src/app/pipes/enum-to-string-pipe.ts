import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'enumToString',
  standalone: true
})
export class EnumToStringPipe implements PipeTransform {
  transform(value: number | undefined, enumObject: any): string {
    if (value === null || value === undefined || !enumObject) {
      return 'Unknown';
    }
    
    // TypeScript numeric enums allow reverse mapping (e.g., UserRole[0] returns "Admin")
    const stringValue = enumObject[value];
    
    // If it exists, add spaces before capital letters (e.g., "PlantManager" -> "Plant Manager")
    return stringValue ? stringValue.replace(/([A-Z])/g, ' $1').trim() : 'Unknown';
  }
}