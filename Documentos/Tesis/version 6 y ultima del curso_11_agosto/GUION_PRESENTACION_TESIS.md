# Guion de presentación de tesis

Duración sugerida: 8–10 minutos, más preguntas.

> Antes de exponer: completar facultad/escuela, carnés y asesor en la portada.

## Diapositiva 1. Portada

Presentamos la implementación y validación funcional de una plataforma web para Transportes Génesis. El trabajo integra rutas, geolocalización y confirmación diaria de asistencia. La defensa diferencia lo observado de lo que todavía requiere pruebas adicionales.

**Idea de cierre:** La exposición se limitará a los resultados documentados.

## Diapositiva 2. Problema y contexto

La operación coordina buses, rutas, paradas, estudiantes, pilotos, monitores y familias. Cuando la información se reparte entre llamadas, mensajes y registros separados, se dificulta consultar la ubicación, la ruta y el estado diario del estudiante. La propuesta centraliza esas funciones por roles.

**Idea de cierre:** La necesidad es centralizar información operativa dispersa.

## Diapositiva 3. Hipótesis y objetivos

La hipótesis se limita al funcionamiento observable. No afirma reducción de tiempos, satisfacción, seguridad física ni adopción productiva. Buscamos verificar si rutas, ubicación y asistencia pueden coexistir y consultarse en una plataforma común.

**Idea de cierre:** La hipótesis evalúa integración funcional, no impacto estadístico.

## Diapositiva 4. Solución y arquitectura

La solución se organiza por capas. Las interfaces se conectan con páginas, controladores y API; los servicios concentran la lógica; Entity Framework Core accede a SQL Server; y SignalR distribuye actualizaciones a los clientes. Leaflet, OpenStreetMap y OSRM apoyan la representación cartográfica.

**Idea de cierre:** La arquitectura conecta interfaces, servicios, datos y mapas.

## Diapositiva 5. Roles y flujo operativo

El administrador configura la operación; piloto y monitor consultan el recorrido; y el padre accede al mapa y confirma asistencia por fecha y turno. La optimización de paradas usa una heurística práctica, por lo que no se presenta como una solución globalmente óptima.

**Idea de cierre:** Cada perfil participa en un flujo operativo diferenciado.

## Diapositiva 6. Geolocalización en tiempo casi real

El navegador obtiene la ubicación del piloto o monitor. La API recibe las coordenadas, el servicio puede persistirlas y SignalR distribuye la actualización al mapa. Usamos el término tiempo casi real porque existe latencia y la precisión depende del dispositivo, permisos y conectividad.

**Idea de cierre:** La ubicación es observable con dependencias técnicas explícitas.

## Diapositiva 7. Asistencia y abordaje

La asistencia representa la intención de utilizar el transporte en una fecha y turno; el abordaje representa una recogida operativa. Los videos respaldan la interfaz de asistencia del padre. No se recibió evidencia inequívoca del monitor guardando y recuperando un registro de abordaje.

**Idea de cierre:** Asistencia observable no equivale a abordaje persistido.

## Diapositiva 8. Metodología y evidencia

La investigación es aplicada, descriptiva y evaluativa bajo un estudio de caso tecnológico. Se revisaron documentos, dos casos funcionales, evidencia audiovisual y arquitectura. No se realizaron encuestas ni se derivaron porcentajes de satisfacción.

**Idea de cierre:** La metodología separa evidencia, interpretación y límites.

## Diapositiva 9. Pruebas y escenarios

El informe del 4 de agosto documenta dos casos del administrador y declara ambos aprobados. Sin embargo, una captura no muestra el mapa esperado y la otra no demuestra una edición guardada. Por eso distinguimos el estado declarado de la verificación visual independiente.

**Idea de cierre:** El informe declara aprobación; las capturas son parciales.

## Diapositiva 10. Resultados y alcance real

El resultado comprobable es la integración observable de rutas, mapas, ubicación y asistencia diaria en un ambiente controlado. El abordaje persistente, el rendimiento, la producción y la aceptación integral no fueron demostrados.

**Idea de cierre:** El alcance real se comunica sin usar métricas no demostradas.

## Diapositiva 11. Evaluación de hipótesis y trabajo futuro

La hipótesis queda respaldada dentro del alcance funcional documentado, no como certificación integral. El siguiente paso es verificar persistencia, ampliar pruebas con padres, pilotos y monitores, evaluar autorizaciones negativas y medir latencia, carga y estabilidad.

**Idea de cierre:** La hipótesis se respalda de forma acotada y verificable.

## Diapositiva 12. Cierre

El aporte verificable es una plataforma integrada y una evidencia funcional trazable, acompañada de límites explícitos. Fueron observables rutas, mapas, geolocalización y la interfaz de asistencia; abordaje y pagos requieren validación adicional. Muchas gracias.

**Idea de cierre:** El principal aporte es integración con trazabilidad documental.

## Respuestas breves ante preguntas previsibles

- **¿La UAT demuestra el sistema completo?** No. El informe documenta dos casos del administrador y declara ambos aprobados; las capturas ofrecen respaldo parcial.
- **¿Por qué no hubo encuestas?** La investigación valida funcionamiento observable con la evidencia disponible; no afirma satisfacción ni aceptación general.
- **¿El abordaje quedó validado?** No de forma persistente. Existe diseño e interfaz, pero falta una prueba que guarde y recupere el registro.
- **¿Es tiempo real?** Se presenta como tiempo casi real porque existe latencia y depende de dispositivo, permisos y conectividad.
- **¿La ruta es óptima?** El orden usa una heurística de vecino más cercano; es práctico, pero no garantiza el óptimo global.
- **¿Los pagos confirman la hipótesis?** No. Son un módulo complementario y no fueron cubiertos por el informe funcional.
