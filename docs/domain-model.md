# Modelo de Dominio – Siniestros

## Agregado Principal: Siniestro

### Atributos
- Id (Guid)
- FechaHora
- Departamento
- Ciudad
- Tipo (Enum)
- VehiculosInvolucrados
- NumeroVictimas
- Descripcion

### Invariantes del dominio
- Departamento y Ciudad son obligatorios
- Número de víctimas no puede ser negativo
- Fecha válida

### Comportamientos
- Crear siniestro
- Actualizar siniestro
- Eliminar siniestro