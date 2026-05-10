Definición de proyecto
001
Seminario Profesional
Seminario Profesional 2
Definición de Proyecto
Definición de Proyecto
(Especificación Funcional, Técnica y Económica)
1. ESPECIFICACIÓN FUNCIONAL
Objetivo: Definir con precisión qué debe hacer el sistema y bajo qué condiciones de
negocio/dominio.
Items obligatorios que debe contener (en orden lógico):
1.1 Introducción y alcance del sistema
1.2 Contexto organizacional y stakeholders (matriz RACI + mapa de poder-interés)
1.3 Análisis de Requerimientos Funcionales
• 1.3.1 Requerimientos funcionales detallados (formato IEEE: ID, Nombre, Descripción,
Precondición, Postcondición, Flujo principal, Flujos alternos/excepcionales)
• 1.3.2 Catálogo completo de Use Cases (diagrama UML + especificación textual de cada
uno)
• 1.3.3 User Stories + Acceptance Criteria (formato Gherkin) y Priorización (MoSCoW o
WSJF)
• 1.3.4 Diagramas de secuencia y de actividad críticos (mínimo 5–8)
1.4 Análisis de Requerimientos No Funcionales
• Tabla de clasificación (ISO/IEC 25010 o FURPS+):
• Performance (tiempos de respuesta, throughput, latencia)
• Seguridad (autenticación, autorización, GDPR/Ley de Datos)
• Escalabilidad y disponibilidad (99.9 % uptime, horizontal scaling)
• Usabilidad y accesibilidad (WCAG 2.2 nivel AA)
• Portabilidad, mantenibilidad, confiabilidad (MTBF, MTTR)
• Métricas cuantificables y pruebas de aceptación asociadas
1.5 Análisis de Dominio
• Glosario de términos del dominio (mínimo 30–50 términos)
• Reglas de negocio (Business Rules) en formato SBVR o Decision Table
1.6 Supuestos, restricciones y dependencias funcionales
Seminario Profesional 2
Definición de Proyecto
2. ESPECIFICACIÓN TÉCNICA
Objetivo: Definir cómo se construirá el sistema desde el punto de vista tecnológico y
arquitectónico.
Items obligatorios:
2.1 Arquitectura de referencia
• Diagrama C4 (Context, Containers, Components, Code)
• Estilo arquitectónico elegido (Microservicios, Clean Architecture, Hexagonal, Event-
Driven, etc.) y justificación
• Diagramas de despliegue (UML) y de componentes
2.2 Stack tecnológico seleccionado
• Frontend, Backend, Mobile (si aplica), Base de datos, Cloud/Infrastructure-as-Code
• Justificación técnica + comparación de alternativas (tabla de trade-offs)
• Versiones exactas y licencias
2.3 Requerimientos de infraestructura y entornos
• Hardware mínimo y recomendado (on-premise o cloud)
• Diagrama de red y topología de seguridad
• Especificación de contenedores (Docker), orquestación (Kubernetes) y CI/CD pipeline
2.4 Diseño de base de datos
• Esquema físico (DDL) + índices, particionamiento, sharding strategy
• Estrategia de migración de datos (si existe sistema legacy)
2.5 Diseño de APIs e integración
• Swagger/OpenAPI 3.0 completo
• Protocolos (REST, GraphQL, gRPC, Kafka) y contratos de interfaz
2.6 Patrones de diseño y buenas prácticas
• Patrones aplicados (Repository, CQRS, Saga, Circuit Breaker, etc.)
• Estrategia de testing (unit, integration, contract, end-to-end, performance) y herramientas
2.7 Plan de implementación y roadmap técnico
• Diagrama de Gantt o Kanban de releases (MVP → v1.0 → futuras)
• Estrategia de DevOps, monitoreo (Prometheus + Grafana) y logging
2.8 Análisis de riesgos técnicos y plan de mitigación (FMEA o tabla de riesgos)
Seminario Profesional 2
Definición de Proyecto
3. ESPECIFICACIÓN ECONÓMICA
Objetivo: Demostrar viabilidad financiera y sostenibilidad del proyecto (obligatorio en maestrías
con enfoque empresarial).
Items obligatorios:
3.1 Resumen ejecutivo económico
3.2 Análisis de costos (Cost Breakdown Structure)
• Costos de desarrollo (esfuerzo en horas × tarifa, desglosado por rol)
• Costos de infraestructura (cloud, servidores, licencias)
• Costos de mantenimiento y soporte (años 1–5)
• Costos de capacitación y cambio organizacional
• Costos indirectos (overhead 15–20 %)
3.3 Estimación de esfuerzo y duración
• Método elegido (COCOMO II, Function Point Analysis, Story Points + Velocity)
• Tabla de estimación detallada + rango ±10 % (P50, P80, P90)
3.4 Modelo de negocio
• Business Model Canvas completo
• Value Proposition Canvas
• Modelo de ingresos (suscripción, freemium, licencia perpetua, pay-per-use, etc.)
• Estrategia de monetización y pricing
3.5 Análisis financiero
• Flujo de caja proyectado (5 años)
• Indicadores clave:
• VAN (Valor Actual Neto)
• TIR (Tasa Interna de Retorno)
• Período de recuperación de inversión (Payback)
• ROI y ROAS
• Punto de equilibrio (Break-even)
• Análisis de sensibilidad (variación ±20 % en costos e ingresos)
3.6 Análisis de mercado y competencia
• TAM / SAM / SOM
• Matriz de competidores y posicionamiento
3.7 Plan de negocio ejecutivo (Executive Summary + 5 páginas)
3.8 Fuentes de financiamiento y estrategia de inversión
3.9 Análisis de riesgos económicos y plan de contingencia (probabilidad × impacto)