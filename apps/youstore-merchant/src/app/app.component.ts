import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'youstore-merchant-root',
  standalone: true,
  imports: [MatToolbarModule, MatButtonModule, MatCardModule],
  template: `
    <mat-toolbar color="primary">
      <span>YouStore Merchant</span>
      <span class="spacer"></span>
      <button mat-button>Dashboard</button>
    </mat-toolbar>

    <div class="container">
      <mat-card>
        <mat-card-title>Create your store</mat-card-title>
        <mat-card-content>
          <p>Register, create a store, and configure your catalog.</p>
        </mat-card-content>
        <mat-card-actions>
          <button mat-raised-button color="accent">Get started</button>
        </mat-card-actions>
      </mat-card>

      <mat-card>
        <mat-card-title>Quick actions</mat-card-title>
        <mat-card-content>
          <p>Manage categories and publish products with confidence.</p>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [
    `
      :host {
        display: block;
        min-height: 100vh;
        background: #f9fafb;
      }

      .spacer {
        flex: 1 1 auto;
      }

      .container {
        padding: 24px;
        display: grid;
        gap: 24px;
      }
    `
  ]
})
export class AppComponent {}
