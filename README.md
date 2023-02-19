# OpenTelemetryDotNetPoc
A small POC project to showcase instrumentation of a Net 6+ applications with OpenTelemetry.  This is an educational experiment and is not meant to represent a production-ready environment.  Some of this information is a little challenging to research and piece together. I thought this POC might be helpful as documentation for me and for those trying to get a jump start.

There are many ways to instrument this tooling. I will update this README.md to describe the overall approach as it evolves.  

## Approach
### Ingredients:
- Net 6 Web Api w/Controllers
- Docker Compose
- System.Diagnostics (uses open telemetry standards)
- OpenTelemetry .net packages
- OpenTelemetry Collector
- Prometheus
- Jaeger
- Grafana
- Influx Db (for Grafana)

### Current Setup and evolving:
- Prometheus exporter used on GatewayApi
- Collector scraping prometheus endpoint
- Collector exposes a new prometheus endpoint
- Prometheus scrapes collector
- Jaeger Exporter used on GatewayApi
- Traces written directly to Jaeger at the moment
- Simulated api response time delays for variation
- Simulated errors with random exceptions thrown (500 status code) for variation
- Api filters to capture metrics with route, class, method, and status code labels to use for querying

### Future Goals:
- Instrument logging stack.  Elk and maybe try Loki
- Instrument Zipkin to compare to Jaeger
- Export metrics, tracing and logging as with an Otel Exporter only and disband Jaeger/Prometheus exporters.
- External tooling to interface exclusively to collector
- Add another more apis and kafka for interesting results
- Some load testing tooling to simulate a constant load

## Docker Instructions:
- Select and run project with Docker Compose Startup project in Visual Studio.  
- Currently experiencing collector scraping issues when using command line docker compose up.  Will fix soon.

## UI Endpoints to check out:
- Swagger: https://localhost:5001/swagger/index.html
- Prometheus: http://localhost:9090/graph
- Jaeger: http://localhost:16686/search
- Grafana: http://localhost:3000

## Grafana Instructions:
- Navigate to http://localhost:3000 and login with admin/admin
- Add prometheus datasource with host: http://poc-prometheus:9090
- Add Jaeger datasource with host: http://poc-jaeger:16686
- Go to /telemetry/grafana folder and copy contents of poc-dashboard.json 
- Create a new dashboard with "Import" and paste json

Includes ability to filter by prometheus metric labels. Jaeger traces filter by the selected time range.

<img src="https://github.com/MetalHexx/OpenTelemetryDotNetPoc/blob/main/assets/grafana.png?raw=true" style=" width:40% ; height:40% " >
