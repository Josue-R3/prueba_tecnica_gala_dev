import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import {
  EmpleadoDto,
  CreateEmpleadoDto,
  UpdateEmpleadoDto,
  PagedResult,
} from '../models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class EmpleadoService {
  private readonly apiUrl = `${environment.apiUrl}/api/empleados`;
  private empleadosSubject = new BehaviorSubject<EmpleadoDto[]>([]);
  public empleados$ = this.empleadosSubject.asObservable();

  constructor(private http: HttpClient) {}

  // Obtener todos los empleados
  getAll(): Observable<EmpleadoDto[]> {
    return this.http
      .get<EmpleadoDto[]>(this.apiUrl)
      .pipe(tap((empleados) => this.empleadosSubject.next(empleados)));
  }

  // Obtener empleados paginados
  getPaginated(
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string
  ): Observable<PagedResult<EmpleadoDto>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<PagedResult<EmpleadoDto>>(`${this.apiUrl}/paginated`, {
      params,
    });
  }

  // Obtener empleado por ID
  getById(id: number): Observable<EmpleadoDto> {
    return this.http.get<EmpleadoDto>(`${this.apiUrl}/${id}`);
  }

  // Buscar empleados
  search(searchTerm: string): Observable<EmpleadoDto[]> {
    const params = new HttpParams().set('searchTerm', searchTerm);
    return this.http.get<EmpleadoDto[]>(`${this.apiUrl}/search`, { params });
  }

  // Crear empleado
  create(empleado: CreateEmpleadoDto): Observable<EmpleadoDto> {
    return this.http
      .post<EmpleadoDto>(this.apiUrl, empleado)
      .pipe(tap(() => this.refreshEmpleados()));
  }

  // Actualizar empleado
  update(id: number, empleado: UpdateEmpleadoDto): Observable<EmpleadoDto> {
    return this.http
      .put<EmpleadoDto>(`${this.apiUrl}/${id}`, empleado)
      .pipe(tap(() => this.refreshEmpleados()));
  }

  // Eliminar empleado
  delete(id: number): Observable<void> {
    return this.http
      .delete<void>(`${this.apiUrl}/${id}`)
      .pipe(tap(() => this.refreshEmpleados()));
  }

  // Método para refrescar la lista de empleados
  private refreshEmpleados(): void {
    this.getAll().subscribe();
  }

  // Obtener empleados activos del subject
  getEmpleadosActivos(): EmpleadoDto[] {
    return this.empleadosSubject.value.filter((emp) => emp.estado);
  }
}
