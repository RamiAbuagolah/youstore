import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'youstore-storefront-root',
  standalone: true,
  imports: [MatToolbarModule, MatCardModule, MatFormFieldModule, MatInputModule, MatIconModule, FormsModule],
  template: `
    <mat-toolbar color="primary">
      <span>YouStore Storefront</span>
    </mat-toolbar>

    <div class="search">
      <mat-form-field appearance="fill">
        <mat-label>Search products</mat-label>
        <input matInput placeholder="Search the marketplace" />
        <mat-icon matSuffix>search</mat-icon>
      </mat-form-field>
    </div>

    <section class="list">
      <mat-card *ngFor="let product of products">
        <mat-card-title>{{ product.name }}</mat-card-title>
        <mat-card-subtitle>{{ product.price | currency:product.currency }}</mat-card-subtitle>
        <mat-card-content>
          <p>{{ product.description }}</p>
        </mat-card-content>
      </mat-card>
    </section>
  `,
  styles: [
    `
      :host {
        display: block;
        background: #fff;
        min-height: 100vh;
      }

      .search {
        padding: 16px;
      }

      .list {
        padding: 16px;
        display: grid;
        gap: 16px;
      }
    `
  ]
})
export class AppComponent {
  products = [
    {
      name: 'Minimalist Lamp',
      description: 'Warm light perfect for any desk.',
      price: 49,
      currency: 'USD'
    },
    {
      name: 'Organic Cotton Tee',
      description: 'Soft fabric, minimal seam details.',
      price: 28,
      currency: 'USD'
    }
  ];
}
