import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { TiendaDto, CreateTiendaDto, UpdateTiendaDto } from '../models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TiendaService {
  private readonly apiUrl = `${environment.apiUrl}/api/tiendas`;
  private tiendasSubject = new BehaviorSubject<TiendaDto[]>([]);
  public tiendas$ = this.tiendasSubject.asObservable();

  constructor(private http: HttpClient) {}

  // Obtener todas las tiendas
  getAll(): Observable<TiendaDto[]> {
    return this.http
      .get<TiendaDto[]>(this.apiUrl)
      .pipe(tap((tiendas) => this.tiendasSubject.next(tiendas)));
  }

  // Obtener solo tiendas activas
  getActivas(): Observable<TiendaDto[]> {
    return this.http
      .get<TiendaDto[]>(`${this.apiUrl}/activas`)
      .pipe(tap((tiendas) => this.tiendasSubject.next(tiendas)));
  }

  // Obtener tienda por ID
  getById(id: number): Observable<TiendaDto> {
    return this.http.get<TiendaDto>(`${this.apiUrl}/${id}`);
  }

  // Crear tienda
  create(tienda: CreateTiendaDto): Observable<TiendaDto> {
    return this.http
      .post<TiendaDto>(this.apiUrl, tienda)
      .pipe(tap(() => this.refreshTiendas()));
  }

  // Actualizar tienda
  update(id: number, tienda: UpdateTiendaDto): Observable<TiendaDto> {
    return this.http
      .put<TiendaDto>(`${this.apiUrl}/${id}`, tienda)
      .pipe(tap(() => this.refreshTiendas()));
  }

  // Eliminar tienda
  delete(id: number): Observable<void> {
    return this.http
      .delete<void>(`${this.apiUrl}/${id}`)
      .pipe(tap(() => this.refreshTiendas()));
  }

  // Método para refrescar la lista de tiendas
  private refreshTiendas(): void {
    this.getAll().subscribe();
  }

  // Obtener tiendas activas del subject
  getTiendasActivas(): TiendaDto[] {
    return this.tiendasSubject.value.filter((tienda) => tienda.estado);
  }
}
